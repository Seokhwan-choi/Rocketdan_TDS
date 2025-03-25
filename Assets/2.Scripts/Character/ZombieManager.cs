using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace TDS
{
    class ZombieManager
    {
        Dictionary<int, Transform> mLineParentDics;
        Dictionary<int, List<Zombie>> mZombieListDics;
        public void Init()
        {
            // 좀비가 이동하는 라인은 총 3개 ( 1 ~ 3 )
            mLineParentDics = new Dictionary<int, Transform>();
            mZombieListDics = new Dictionary<int, List<Zombie>>();
            for (int i = 0; i < 3; ++i)
            {
                int line = i + 1;

                // 좀비가 라인별로 생성되는 오브젝트 부모
                if (mLineParentDics.ContainsKey(line) == false)
                {
                    var lineParent = GameObject.Find($"Line_{line:00}").transform;

                    mLineParentDics.Add(line, lineParent);

                    var sortingGroup = lineParent.GetComponent<SortingGroup>();

                    // 라인을 기준으로 댑스 정렬
                    sortingGroup.sortingOrder = -Mathf.FloorToInt(lineParent.position.y * 100f);
                }

                // 좀비를 라인별로 저장하는 리스트
                if (mZombieListDics.ContainsKey(line) == false)
                {
                    mZombieListDics.Add(line, new List<Zombie>());
                }
            }
        }

        public void OnUpdate(float dt)
        {
            foreach(var zombieList in mZombieListDics.Values)
            {
                zombieList.ForEach(x => { if (x.IsDeath == false)
                    {
                        x.OnUpdate(dt);
                    }
                });
            }
        }

        public void OnFixedUpdate(float dt)
        {
            foreach (var zombieList in mZombieListDics.Values)
            {
                zombieList.ForEach(x =>
                {
                    if (x.IsDeath == false)
                    {
                        x.OnFixedUpdate(dt);
                    }
                });
            }
        }

        public void  SmoothKnockBackZombies(int line, int floor)
        {
            var zombieList = mZombieListDics.TryGet(line);
            var zombieListWhere = zombieList.Where(x => x.Floor == floor);
            foreach (Zombie zombie in zombieListWhere)
            {
                if (zombie.InSmoothKnockback)
                    continue;

                if (zombie.Action.IsActiveMove || zombie.InJumping)
                    continue;

                zombie.CharacterPhysics.SmoothKnockback();
            }
        }

        public void RemoveZombie(Zombie zombie)
        {
            int line = zombie.Line;

            var zombieList = mZombieListDics.TryGet(line);

            if (zombieList.Contains(zombie))
            {
                zombieList.Remove(zombie);

                zombie.OnRelease();

                TDS.ObjectPool.ReleaseObject(zombie.gameObject);
            }
        }

        public void SpawnZombie(int line)
        {
            GameObject zombieObj = TDS.ObjectPool.AcquireObject($"Monster/ZombieMelee", mLineParentDics.TryGet(line));

            Zombie zombie = zombieObj.GetOrAddComponent<Zombie>();
            zombie.Init(this, line);
            zombie.SetLocalPosition(Vector2.right * 10f);

            var zombieList = mZombieListDics.TryGet(line);
            zombieList?.Add(zombie);
        }

        public Zombie FindClosestZombie(Vector2 refPos)
        {
            Zombie closestZombie = null;
            float minDistance = float.MaxValue;

            foreach(var zombieList in mZombieListDics.Values)
            {
                foreach(var zombie in zombieList)
                {
                    if (zombie.IsDeath)
                        continue;

                    float distance = Vector2.Distance(refPos, zombie.GetPosition());
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestZombie = zombie;
                    }
                }
            }

            return closestZombie;
        }

        public bool HaveFloorZombie(int line)
        {
            var zombieList = mZombieListDics.TryGet(line);
            foreach (var zombie in zombieList)
            {
                if (zombie.IsDeath)
                    continue;

                if (zombie.InJumping)
                    continue;

                if (zombie.Floor > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsBackMostZombie(Zombie target)
        {
            if (target.IsDeath)
                return false;

            Zombie backMostZombie = null;
            float backMostPosX = 0f;

            var zombieList = mZombieListDics.TryGet(target.Line);
            var zombieListWhere = zombieList.Where(x => x.Floor == target.Floor);

            foreach (var zombie in zombieListWhere)
            {
                if (zombie.IsDeath)
                    continue;

                if (zombie.InJumping || zombie.InSmoothKnockback)
                    continue;

                if (zombie.Action.IsActiveMove)
                    continue;

                float zombiePosX = zombie.GetPosition().x;

                if (backMostPosX < zombiePosX)
                {
                    backMostZombie = zombie;
                    backMostPosX = zombiePosX;
                }
            }

            return backMostZombie == target;
        }
    }
}