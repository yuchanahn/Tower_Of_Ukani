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

        public FollowingData FindPath(Vector3 ori, Vector3 target)
        {
            var ori_vi3 = linkMap.NavMesh.grid.WorldToCell(ori);
            var tar_vi3 = linkMap.NavMesh.grid.WorldToCell(target);
            origin = new Vector2Int(ori_vi3.x, ori_vi3.y);
            destination = new Vector2Int(tar_vi3.x, tar_vi3.y);

            var navmesh = linkMap.NavMesh;
            path = AStar.CalculatePath(
                navmesh.IndexOf(origin.x, origin.y),
                navmesh.IndexOf(destination.x, destination.y),
                linkMap
            );
            if (path == null || path.Count < 3) return new FollowingData(false, false, Vector2.zero);


            var link = FindLinkFromTo(path[1], path[2]);
            bool bLink = FindLinkFromTo(path[1], path[2]) is KuroiLinkMap.LinkNode.GravitationalLink;

            bool linked = false;
            for (int i = 0; i < path.Count - 1; i++)
            {
                if (!linked)
                {
                    linked = FindLinkFromTo(path[i], path[i + 1]) is KuroiLinkMap.LinkNode.GravitationalLink;
                    if(linked)  print($"Jump\n{i},{i+1}");
                    // Jump 0, 1 에서도 가능... 
                }
            }
            
            Vector2 Ori_CellToWorld = navmesh.grid.GetCellCenterWorld((Vector3Int)navmesh.PositionOf(path[0]));
            Vector2 Ori_CellToWorld2 = navmesh.grid.GetCellCenterWorld((Vector3Int)navmesh.PositionOf(path[1]));
            Vector2 max = Ori_CellToWorld;
            if (link is KuroiLinkMap.LinkNode.GravitationalLink g)
            {
                
                var points = g.Path;
                foreach(var i in g.Path)
                {
                    max = i.y > max.y ? i : max;
                }
                
            }
            Vector2 oPos = (navmesh.grid.CellToWorld((Vector3Int)navmesh.PositionOf(path[2])) - navmesh.grid.CellToWorld((Vector3Int)navmesh.PositionOf(path[0]))).normalized;


            

            oPos = bLink ? (max - Ori_CellToWorld2) : oPos;
            if (bLink)
            {
                print($"{(max - Ori_CellToWorld2)}\n pos1 {Ori_CellToWorld}\n pos2{Ori_CellToWorld2}\n tar{max}");
            }
            oPos.x *= 3;
            return new FollowingData(true, bLink, oPos);
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