using System.Collections.Generic;
using System.Linq;
using Shiroi.Pathfinding2D.Kuroi;
using Shiroi.Pathfinding2D.Runtime;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Pathfinding2D.Examples
{
    public class PathFinder : MonoBehaviour
    {
        [SerializeField] GameObject LinkCheck;
        [SerializeField] GameObject LinkMaxCheck;

        public static PathFinder Inst;

        public Vector2Int origin, destination;
        public List<uint> path;

        public KuroiLinkMap linkMap;
        public Grid Grid => linkMap.NavMesh.grid;

        public void Awake()
        {
            Inst = this;
        }

        public void SwapPoints()
        {
            var t = origin;
            origin = destination;
            destination = t;
        }

        public FollowingData FindPath(Vector3 oriPos, Vector3 oPos, Vector3 tPos)
        {
            #region Init
            var ori_vi3 = linkMap.NavMesh.grid.WorldToCell(oPos);
            var tar_vi3 = linkMap.NavMesh.grid.WorldToCell(tPos);
            this.origin = new Vector2Int(ori_vi3.x, ori_vi3.y);
            destination = new Vector2Int(tar_vi3.x, tar_vi3.y);

            var navmesh = linkMap.NavMesh;
            path = AStar.CalculatePath(
                navmesh.IndexOf(this.origin.x, this.origin.y),
                navmesh.IndexOf(destination.x, destination.y),
                linkMap
            );
            #endregion;

            // 경로가 없다면 팔로우 하지 않는다.
            if (path == null)
            {
                return new FollowingData(false, false, Vector2.zero);
            }

            var link = FindLinkFromTo(path[0], path[1]);

            Vector2 jumpMaxHeight = oPos;
            if (link is KuroiLinkMap.LinkNode.GravitationalLink g)
            {
                foreach (var i in g.Path)
                {
                    jumpMaxHeight = i.y > jumpMaxHeight.y ? i : jumpMaxHeight;
                }
                var lastx = g.Path[g.Path.Length - 1];

                jumpMaxHeight = new Vector2(lastx.x, jumpMaxHeight.y);
                var non = jumpMaxHeight - new Vector2(oPos.x, oPos.y);


                return new FollowingData(true, true, non);

            }
            else  // 링크(점프)할 수 없다면 가야 하는 방향을 리턴한다.
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    if (FindLinkFromTo(path[i], path[i+1]) is KuroiLinkMap.LinkNode.GravitationalLink g2)
                    {
                        tPos = g2.Path[0];
                        break;
                    }
                }

                return new FollowingData(true, false, tPos - oPos);
            }
        }






        bool find = false;
        public void FindPath()
        {
            var navmesh = linkMap.NavMesh;
            path = AStar.CalculatePath(
                navmesh.IndexOf(origin.x, origin.y),
                navmesh.IndexOf(destination.x, destination.y),
                linkMap
            );
            find = true;
        }


        private void OnDrawGizmos()
        {
            var navmesh = linkMap.NavMesh;
            if (navmesh == null || navmesh.grid == null)
            {
                return;
            }

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(navmesh.grid.GetCellCenterWorld((Vector3Int)origin), 1F);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(navmesh.grid.GetCellCenterWorld((Vector3Int)destination), 1);
            Gizmos.color = Color.yellow;
            if (path == null)
            {
                return;
            }

            var color = new Color(1f, 0.67f, 0f);
            for (int i = 0; i < path.Count - 1; i++)
            {
                var oPos = navmesh.grid.CellToWorld((Vector3Int)navmesh.PositionOf(path[i + 1]));
                var link = FindLinkFromTo(path[i], path[i + 1]);

                Gizmos.color = color;

                if (link is KuroiLinkMap.LinkNode.GravitationalLink g)
                {
                    var points = g.Path;

                    for (var j = 0; j < points.Length - 1; j++)
                    {
                        var a = points[j];
                        var b = points[j + 1];
                        Gizmos.DrawLine(
                            a,
                            b
                        );
                    }



                    continue;
                }
                Gizmos.DrawLine(
                    oPos,
                    navmesh.grid.GetCellCenterWorld((Vector3Int)navmesh.PositionOf(path[i]))
                );
            }

        }

        private ILink FindLinkFromTo(uint from, uint to)
        {
            return linkMap.nodes[from].Links.FirstOrDefault(
                link => link.Destination == to
            );
        }
    }
}