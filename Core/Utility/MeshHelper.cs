using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace NonsensicalKit.Utility
{
    /// <summary>
    /// mesh操作工具类
    /// </summary>
    public static class MeshHelper
    {
        /// <summary>
        /// 清除未被使用的顶点
        /// </summary>
        /// <param name="mesh"></param>
        public static void ClearUnuseVertex(Mesh mesh)
        {
            //值类型转引用类型存储
            List<Vector3?> vertexsQuote = new List<Vector3?>();
            List<Vector2> uv = new List<Vector2>(mesh.uv);
            List<Vector3> normals = new List<Vector3>(mesh.normals);

            foreach (var item in mesh.vertices)
            {
                vertexsQuote.Add(item);
            }

            //保存引用类型
            List<TriangleContainer> triangleContainers = new List<TriangleContainer>();

            foreach (var item in mesh.triangles)
            {
                triangleContainers.Add(new TriangleContainer(vertexsQuote[item]));
            }

            //获取未被使用的定点
            bool[] use = Enumerable.Repeat(false, mesh.vertexCount).ToArray();

            foreach (var item in mesh.triangles)
            {
                use[item] = true;
            }

            //从链表中清除未被使用的定点
            for (int i = 0, useCount = 0; i < vertexsQuote.Count; i++, useCount++)
            {
                if (use[useCount] == false)
                {
                    vertexsQuote.RemoveAt(i);
                    uv.RemoveAt(i);
                    normals.RemoveAt(i);
                    i--;
                }
            }

            //获取新三角数组
            List<int> triangles = new List<int>();

            foreach (var item in triangleContainers)
            {
                triangles.Add(vertexsQuote.IndexOf(item.Vertice));
            }

            //链表转数组
            List<Vector3> vertices = new List<Vector3>();
            foreach (var item in vertexsQuote)
            {
                vertices.Add((Vector3)item);
            }

            //赋值mesh
            mesh.triangles = triangles.ToArray();
            mesh.vertices = vertices.ToArray();
            mesh.uv = uv.ToArray();
            mesh.normals = normals.ToArray();
        }

        /// <summary>
        /// 根据顶点数组求出质点并返回回偏移量
        /// </summary>
        /// <param name="mesh">需要求质点的mesh</param>
        /// <returns></returns>
        public static Vector3 AutoCentroidShift(Mesh mesh)
        {
            Vector3[] vertices = mesh.vertices;

            Vector3 sum = Vector3.zero;

            foreach (var item in vertices)
            {
                sum += item;
            }

            Vector3 offSet = -sum / vertices.Length;

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] += offSet;
            }

            return offSet;
        }

        /// <summary>
        /// 网格是否包含坐标,性能较低，但可以判断凹体网格或者三角面方向不正确的网格
        /// </summary>
        public static bool Contain(Vector3[] vertices, int[] triangle, Vector3 pos_Local)
        {

            Vector3 dir = Vector3.up;

            List<Vector3> crossPoint = new List<Vector3>();
            for (int i = 0; i < triangle.Length; i += 3)
            {
                Vector3? cross = VectorHelper.GetRayTriangleCrossPoint(pos_Local, dir, vertices[triangle[i]], vertices[triangle[i + 1]], vertices[triangle[i + 2]]);
                if (cross != null)
                {
                    crossPoint.Add((Vector3)cross);
                }
            }
            VectorHelper.SortList(crossPoint);

            for (int i = 0; i < crossPoint.Count - 1; i++)
            {
                if (VectorHelper.IsNear(crossPoint[i], crossPoint[i + 1]))
                {
                    crossPoint.RemoveAt(i + 1);
                    i--;
                }
            }

            if (crossPoint.Count % 2 == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool Contain(this Mesh mesh, Vector3 pos_Local)
        {
            Vector3[] vertices = mesh.vertices;
            int[] triangle = mesh.triangles;
            return Contain(vertices, triangle, pos_Local);
        }

        public static bool Contain(this MeshBuffer mesh, Float3 pos_Local)
        {
            return Contain(mesh, pos_Local.ToVector3());
        }

        public static bool Contain(this MeshBuffer mesh, Vector3 pos_Local)
        {
            Vector3[] vertices = mesh.vertices.ToArray();
            int[] triangle = mesh.triangles.ToArray();

            return Contain(vertices, triangle, pos_Local);
        }

        /// <summary>
        /// 网格是否包含坐标，性能较高，但要求网格为非凹体且网格三角面方向完全正确
        /// </summary>
        public static bool Contain_2(Vector3[] vertices, int[] triangle, Vector3 pos_Local)
        {

            for (int i = 0; i < triangle.Length; i += 3)
            {
                Vector3 line1 = vertices[triangle[i + 1]] - vertices[triangle[i]];
                Vector3 line2 = vertices[triangle[i + 2]] - vertices[triangle[i]];

                Vector3 normal = Vector3.Cross(line1, line2);

                Vector3 posDir = pos_Local - vertices[triangle[i + 1]];

                if (Vector3.Dot(posDir, normal) > 0)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool Contain_2(this Mesh mesh, Vector3 pos_Local)
        {
            Vector3[] vertices = mesh.vertices;
            int[] triangle = mesh.triangles;

            return Contain_2(vertices, triangle, pos_Local);
        }

        public static bool Contain_2(this MeshBuffer mesh, Vector3 pos_Local)
        {
            Vector3[] vertices = mesh.vertices.ToArray();
            int[] triangle = mesh.triangles.ToArray();

            return Contain_2(vertices, triangle, pos_Local);
        }

        /// <summary>
        /// 进行包含位移变换的矩阵转换
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector3 Operator(Matrix4x4 lhs, Vector3 vector)
        {
            return lhs * new Vector4(vector.x, vector.y, vector.z, 1);
        }

        /// <summary>
        /// 返回一个刚好包住所有子物体的bounds(世界方向)
        /// </summary>
        /// <param name="go"></param>
        public static Bounds GetBoundsWithChild(GameObject go)
        {
            Quaternion old = go.transform.rotation;
            go.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

            bool hasBounds = false;
            Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);

            Renderer[] childRenderers = go.transform.GetComponentsInChildren<Renderer>();

            foreach (var item in childRenderers)
            {
                if (hasBounds)
                {
                    bounds.Encapsulate(item.bounds);
                }
                else
                {
                    bounds = item.bounds;
                    hasBounds = true;
                }
            }
            bounds.center = bounds.center - go.transform.position;

            bounds.size = new Vector3(bounds.size.x / go.transform.lossyScale.x, bounds.size.y / go.transform.lossyScale.y, bounds.size.z / go.transform.lossyScale.z);

            go.transform.rotation = old;
            return bounds;
        }

        private class TriangleContainer
        {
            public Vector3? Vertice;

            public TriangleContainer(Vector3? _vertice)
            {
                Vertice = _vertice;
            }
        }
    }

    /// <summary>
    /// 用于缓存mesh数据以进行处理
    /// </summary>
    public class MeshBuffer
    {
        public List<Vector3> vertices;
        public List<Vector3> normals;
        public List<Color> colors;
        public List<Vector2> uv;
        public List<Vector2> uv2;
        public List<Vector2> uv3;
        public List<int> triangles;
      
        private Dictionary<uint, int> newVectices;

        public MeshBuffer(Mesh mesh)
        {
            vertices = new List<Vector3>(mesh.vertices);
            normals = new List<Vector3>(mesh.normals);
            colors = new List<Color>(mesh.colors);
            uv = new List<Vector2>(mesh.uv);
            uv2 = new List<Vector2>(mesh.uv2);
            uv3 = new List<Vector2>(mesh.uv3);
            triangles = new List<int>(mesh.triangles);
        }

        public MeshBuffer()
        {
            vertices = new List<Vector3>();
            normals = new List<Vector3>();
            colors = new List<Color>();
            uv = new List<Vector2>();
            uv2 = new List<Vector2>();
            uv3 = new List<Vector2>();
            triangles = new List<int>();
        }

        public Vector3 GetVerticeByTrianglesIndex(int _index)
        {
            return vertices[triangles[_index]];
        }

        public Vector3 this[int index]
        {
            get
            {
                return vertices[triangles[index]];
            }
        }

        public MeshBuffer Clone()
        {
            MeshBuffer temp = new MeshBuffer()
            {
                vertices = new List<Vector3>(this.vertices.ToArray()),
                normals = new List<Vector3>(this.normals.ToArray()),
                colors = new List<Color>(this.colors.ToArray()),
                uv = new List<Vector2>(this.uv.ToArray()),
                uv2 = new List<Vector2>(this.uv2.ToArray()),
                uv3 = new List<Vector2>(this.uv3.ToArray()),
                triangles = new List<int>(this.triangles.ToArray())
            };

            return temp;
        }

        public void Apply(Mesh mesh)
        {
            mesh.Clear();
            mesh.SetVertices(vertices);
            mesh.SetNormals(normals);
            mesh.SetColors(colors);
            mesh.SetUVs(0, uv);
            mesh.SetUVs(1, uv2);
            mesh.SetUVs(2, uv3);
            mesh.SetTriangles(triangles, 0);
        }

        public Mesh ToMesh()
        {
            Mesh mesh = new Mesh();

            mesh.SetVertices(vertices);
            mesh.SetNormals(normals);
            mesh.SetColors(colors);
            mesh.SetUVs(0, uv);
            mesh.SetUVs(1, uv2);
            mesh.SetUVs(2, uv3);
            mesh.SetTriangles(triangles, 0);
            //mesh.RecalculateNormals();    //不使用时效果更加平滑
            return mesh;
        }

        public void Scale(Vector3 scaleRatio)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] = Vector3.Scale(vertices[i], scaleRatio);
            }
        }

        public void AddTriangle(Vector3[] _vertices, Vector3 _normal, Vector2 _uv)
        {
            int rawLength = vertices.Count;

            vertices.Add(_vertices[0]);
            vertices.Add(_vertices[1]);
            vertices.Add(_vertices[2]);

            normals.Add(_normal);
            normals.Add(_normal);
            normals.Add(_normal);

            uv.Add(_uv);
            uv.Add(_uv);
            uv.Add(_uv);

            triangles.Add(rawLength + 0);
            triangles.Add(rawLength + 1);
            triangles.Add(rawLength + 2);
        }
        public void AddCube(Float3 center, Float3 size)
        {
            AddCube(center.ToVector3(), size.ToVector3());
        }
        public void AddCube( Vector3 size)
        {
            Vector3[] point = new Vector3[8];
     
            float sizeX = size.x * 0.5f;
            float sizeY = size.y * 0.5f;
            float sizeZ = size.z * 0.5f;
            point[0] = new Vector3( sizeX,  sizeY, sizeZ);
            point[1] = new Vector3( sizeX,  sizeY, sizeZ);
            point[2] = new Vector3( sizeX,  sizeY, sizeZ);
            point[3] = new Vector3( sizeX,  sizeY, sizeZ);
            point[4] = new Vector3( sizeX,  sizeY, sizeZ);
            point[5] = new Vector3( sizeX,  sizeY, sizeZ);
            point[6] = new Vector3( sizeX,  sizeY, sizeZ);
            point[7] = new Vector3( sizeX,  sizeY, sizeZ);

            //init
            Vector2 middle = Vector2.one * 0.5f;

            //front
            AddQuad(Vector3.back, middle, point[0], point[1], point[2], point[3]);

            //back
            AddQuad(Vector3.forward, middle, point[7], point[6], point[5], point[4]);

            //left
            AddQuad(Vector3.left, middle, point[4], point[5], point[1], point[0]);

            //right
            AddQuad(Vector3.right, middle, point[3], point[2], point[6], point[7]);

            //down
            AddQuad(Vector3.down, middle, point[0], point[3], point[7], point[4]);

            //up
            AddQuad(Vector3.up, middle, point[1], point[5], point[6], point[2]);
        }

        public void AddCube(Vector3 center, Vector3 size)
        {
            Vector3[] point = new Vector3[8];
            float centerX = center.x;
            float centerY = center.y;
            float centerZ = center.z;
            float sizeX = size.x*0.5f;
            float sizeY = size.y * 0.5f;
            float sizeZ = size.z * 0.5f;
            point[0] = new Vector3(centerX - sizeX, centerY - sizeY, centerZ - sizeZ);
            point[1] = new Vector3(centerX - sizeX, centerY + sizeY, centerZ - sizeZ);
            point[2] =  new Vector3(centerX + sizeX, centerY + sizeY, centerZ - sizeZ);
            point[3] =new Vector3(centerX + sizeX, centerY - sizeY, centerZ - sizeZ);
            point[4] =  new Vector3(centerX - sizeX, centerY - sizeY, centerZ + sizeZ);
            point[5] = new Vector3(centerX - sizeX, centerY + sizeY, centerZ + sizeZ);
            point[6] =  new Vector3(centerX + sizeX, centerY + sizeY, centerZ + sizeZ);
            point[7] =  new Vector3(centerX + sizeX, centerY - sizeY, centerZ + sizeZ);

            //init
            Vector2 middle = Vector2.one * 0.5f;

            //front
            AddQuad( Vector3.back, middle, point[0], point[1], point[2], point[3]);

            //back
            AddQuad( Vector3.forward, middle,point[7], point[6], point[5], point[4]);

            //left
            AddQuad( Vector3.left, middle, point[4], point[5], point[1], point[0]);

            //right
            AddQuad( Vector3.right, middle, point[3], point[2], point[6], point[7]);

            //down
            AddQuad( Vector3.down, middle, point[0], point[3], point[7], point[4]);

            //up
            AddQuad( Vector3.up, middle, point[1], point[5], point[6], point[2]);
        }

        public void AddQuad( Vector3 _normal, Vector2 _uv,params Vector3[] _vertices)
        {
            int rawLength = vertices.Count;

            vertices.Add(_vertices[0]);
            vertices.Add(_vertices[1]);
            vertices.Add(_vertices[2]);
            vertices.Add(_vertices[3]);

            normals.Add(_normal);
            normals.Add(_normal);
            normals.Add(_normal);
            normals.Add(_normal);

            uv.Add(_uv);
            uv.Add(_uv);
            uv.Add(_uv);
            uv.Add(_uv);

            triangles.Add(rawLength + 0);
            triangles.Add(rawLength + 1);
            triangles.Add(rawLength + 3);

            triangles.Add(rawLength + 1);
            triangles.Add(rawLength + 2);
            triangles.Add(rawLength + 3);
        }

        public void AddQuad(Vector3[] _vertices, Vector3 _normal, Vector2 _uv)
        {
            int rawLength = vertices.Count;

            vertices.Add(_vertices[0]);
            vertices.Add(_vertices[1]);
            vertices.Add(_vertices[2]);
            vertices.Add(_vertices[3]);

            normals.Add(_normal);
            normals.Add(_normal);
            normals.Add(_normal);
            normals.Add(_normal);

            uv.Add(_uv);
            uv.Add(_uv);
            uv.Add(_uv);
            uv.Add(_uv);

            triangles.Add(rawLength + 0);
            triangles.Add(rawLength + 1);
            triangles.Add(rawLength + 3);

            triangles.Add(rawLength + 1);
            triangles.Add(rawLength + 2);
            triangles.Add(rawLength + 3);
        }

        public void AddQuad_2(Vector3[] _vertices, Vector3 _center, Vector2 _uv)
        {
            int rawLength = vertices.Count;

            vertices.Add(_vertices[0]);
            vertices.Add(_vertices[1]);
            vertices.Add(_vertices[2]);
            vertices.Add(_vertices[3]);

            normals.Add(_vertices[0] - _center);
            normals.Add(_vertices[1] - _center);
            normals.Add(_vertices[2] - _center);
            normals.Add(_vertices[3] - _center);

            uv.Add(_uv);
            uv.Add(_uv);
            uv.Add(_uv);
            uv.Add(_uv);

            triangles.Add(rawLength + 0);
            triangles.Add(rawLength + 1);
            triangles.Add(rawLength + 3);

            triangles.Add(rawLength + 1);
            triangles.Add(rawLength + 2);
            triangles.Add(rawLength + 3);
        }

        public void AddQuad_3(Vector3[] _vertices, Vector3[] _centers, Vector2 _uv)
        {
            int rawLength = vertices.Count;

            vertices.Add(_vertices[0]);
            vertices.Add(_vertices[1]);
            vertices.Add(_vertices[2]);
            vertices.Add(_vertices[3]);

            normals.Add(_vertices[0] - _centers[0]);
            normals.Add(_vertices[1] - _centers[1]);
            normals.Add(_vertices[2] - _centers[2]);
            normals.Add(_vertices[3] - _centers[3]);

            uv.Add(_uv);
            uv.Add(_uv);
            uv.Add(_uv);
            uv.Add(_uv);

            triangles.Add(rawLength + 0);
            triangles.Add(rawLength + 1);
            triangles.Add(rawLength + 3);

            triangles.Add(rawLength + 1);
            triangles.Add(rawLength + 2);
            triangles.Add(rawLength + 3);
        }

        public void AddRound(Vector3 center, float radius, Vector3 dir, int smoothness)
        {
            if (smoothness < 3)
            {
                throw new Exception("点数过少");
            }
            Vector3 dir1 = VectorHelper.GetCommonVerticalLine(dir, dir);
            Vector3 dir2 = VectorHelper.GetCommonVerticalLine(dir, dir1);
            float partAngle = (2 * Mathf.PI) / smoothness;
            Vector3[] pointArray = new Vector3[smoothness];

            for (int i = 0; i < smoothness; i++)
            {
                pointArray[i] = center + radius * dir1 * Mathf.Sin(partAngle * i) + radius * dir2 * Mathf.Cos(partAngle * i);
            }
            for (int i = 0; i < pointArray.Length; i++)
            {
                int next = i + 1;
                if (next >= smoothness)
                {
                    next = 0;
                }
                AddTriangle(new Vector3[] { center, pointArray[i], pointArray[next] }, dir, Vector2.one * 0.5f);
            }
        }

        public void AddRing(Vector3 center, float innerDiameter, float outerDiameter, Vector3 dir, int smoothness)
        {
            if (smoothness < 3)
            {
                throw new Exception("点数过少");
            }
            if (innerDiameter == 0)
            {
                AddRound(center, outerDiameter, dir, smoothness);
            }

            Vector3 dir1 = VectorHelper.GetCommonVerticalLine(dir, dir);
            Vector3 dir2 = VectorHelper.GetCommonVerticalLine(dir, dir1);
            float partAngle = (2 * Mathf.PI) / smoothness;
            Vector3[] pointArray1 = new Vector3[smoothness];
            Vector3[] pointArray2 = new Vector3[smoothness];

            for (int i = 0; i < smoothness; i++)
            {
                pointArray1[i] = center + innerDiameter * dir1 * Mathf.Sin(partAngle * i) + innerDiameter * dir2 * Mathf.Cos(partAngle * i);
                pointArray2[i] = center + outerDiameter * dir1 * Mathf.Sin(partAngle * i) + outerDiameter * dir2 * Mathf.Cos(partAngle * i);
            }

            for (int i = 0; i < smoothness; i++)
            {
                int next = i + 1;
                if (next >= smoothness)
                {
                    next = 0;
                }

                //AddQuad(new Vector3[] { pointArray1[i], pointArray2[i], pointArray2[next], pointArray1[next] }, (pointArray1[i] + pointArray2[i] + pointArray2[next] + pointArray1[next]) * 0.25f, new Vector2(0.5f, 0.5f));
                AddQuad(new Vector3[] { pointArray1[i], pointArray2[i], pointArray2[next], pointArray1[next] }, -dir, new Vector2(0.5f, 0.5f));
            }
        }


        public void AddRing3D(Vector3 ringSide1, float ringSide1Radius, Vector3 ringSide2, float ringSide2Radius, Vector3 dir, int smoothness)
        {
            if (smoothness < 3)
            {
                throw new Exception("点数过少");
            }

            Vector3 dir1 = VectorHelper.GetCommonVerticalLine(dir, dir);
            Vector3 dir2 = VectorHelper.GetCommonVerticalLine(dir, dir1);

            float partAngle = (2 * Mathf.PI) / smoothness;
            Vector3[] pointArray1 = new Vector3[smoothness];
            Vector3[] pointArray2 = new Vector3[smoothness];
            for (int i = 0; i < smoothness; i++)
            {
                pointArray1[i] = ringSide1 + ringSide1Radius * dir1 * Mathf.Sin(partAngle * i) + ringSide1Radius * dir2 * Mathf.Cos(partAngle * i);
                pointArray2[i] = ringSide2 + ringSide2Radius * dir1 * Mathf.Sin(partAngle * i) + ringSide2Radius * dir2 * Mathf.Cos(partAngle * i);
            }

            for (int i = 0; i < smoothness; i++)
            {
                int next = i + 1;
                if (next >= smoothness)
                {
                    next = 0;
                }
                // AddQuad(new Vector3[] { pointArray1[i], pointArray2[i], pointArray2[next], pointArray1[next] }, (pointArray1[i] + pointArray2[i] + pointArray2[next] + pointArray1[next]) * 0.25f- (ringSide1+ringSide2)*0.5f, new Vector2(0.5f, 0.5f));
                AddQuad_3(new Vector3[] { pointArray1[i], pointArray2[i], pointArray2[next], pointArray1[next] }, new Vector3[] { ringSide1, ringSide2, ringSide2, ringSide1 }, new Vector2(0.5f, 0.5f));
            }
        }

        /// <summary>
        /// 用两个不平行的圆相连作成环
        /// </summary>
        /// <param name="ringSide1"></param>
        /// <param name="ringSide1Radius"></param>
        /// <param name="dir2"></param>
        /// <param name="ringSide2"></param>
        /// <param name="ringSide2Radius"></param>
        /// <param name="dir2"></param>
        /// <param name="smoothness"></param>
        /// <exception cref="Exception"></exception>
        public void AddRing3D(Vector3 ringSide1, float ringSide1Radius, Vector3 d1, Vector3 ringSide2, float ringSide2Radius, Vector3 d2, int smoothness)
        {
            if (smoothness < 3)
            {
                throw new Exception("点数过少");
            }

            Vector3 d3= VectorHelper.GetCommonVerticalLine(d1, d2);
            Vector3 d1V = VectorHelper.GetCommonVerticalLine(d1, d3);
            Vector3 d2V = VectorHelper.GetCommonVerticalLine(d2, d3);

            float partAngle = (2 * Mathf.PI) / smoothness;
            Vector3[] pointArray1 = new Vector3[smoothness];
            Vector3[] pointArray2 = new Vector3[smoothness];

            for (int i = 0; i < smoothness; i++)
            {
                pointArray1[i] = ringSide1 + ringSide1Radius * d3 * Mathf.Sin(partAngle * i) + ringSide1Radius * d1V * Mathf.Cos(partAngle * i);
                pointArray2[i] = ringSide2 + ringSide2Radius * d3 * Mathf.Sin(partAngle * i) + ringSide2Radius * d2V * Mathf.Cos(partAngle * i);
            }

            for (int i = 0; i < smoothness; i++)
            {
                int next = i + 1;
                if (next >= smoothness)
                {
                    next = 0;
                }
                // AddQuad(new Vector3[] { pointArray1[i], pointArray2[i], pointArray2[next], pointArray1[next] }, (pointArray1[i] + pointArray2[i] + pointArray2[next] + pointArray1[next]) * 0.25f- (ringSide1+ringSide2)*0.5f, new Vector2(0.5f, 0.5f));
                AddQuad_3(new Vector3[] { pointArray1[i], pointArray2[i], pointArray2[next], pointArray1[next] }, new Vector3[] { ringSide1, ringSide2, ringSide2, ringSide1 }, new Vector2(0.5f, 0.5f));
            }
        }

        #region Subdivide4 (2x2)
        private int GetNewVertex4(int i1, int i2)
        {
            int newIndex = vertices.Count;
            uint t1 = ((uint)i1 << 16) | (uint)i2;
            uint t2 = ((uint)i2 << 16) | (uint)i1;
            if (newVectices.ContainsKey(t2))
                return newVectices[t2];
            if (newVectices.ContainsKey(t1))
                return newVectices[t1];

            newVectices.Add(t1, newIndex);

            vertices.Add((vertices[i1] + vertices[i2]) * 0.5f);
            if (normals.Count > 0)
                normals.Add((normals[i1] + normals[i2]).normalized);
            if (colors.Count > 0)
                colors.Add((colors[i1] + colors[i2]) * 0.5f);
            if (uv.Count > 0)
                uv.Add((uv[i1] + uv[i2]) * 0.5f);
            if (uv2.Count > 0)
                uv2.Add((uv2[i1] + uv2[i2]) * 0.5f);
            if (uv3.Count > 0)
                uv3.Add((uv3[i1] + uv3[i2]) * 0.5f);

            return newIndex;
        }


        /// <summary>
        /// Devides each triangles into 4. A quad(2 tris) will be splitted into 2x2 quads( 8 tris )
        /// </summary>
        /// <param name="mesh"></param>
        public  void Subdivide4()
        {
            newVectices = new Dictionary<uint, int>();

            
            for (int i = 0; i < triangles.Count; i += 3)
            {
                int i1 = triangles[i + 0];
                int i2 = triangles[i + 1];
                int i3 = triangles[i + 2];

                int a = GetNewVertex4(i1, i2);
                int b = GetNewVertex4(i2, i3);
                int c = GetNewVertex4(i3, i1);
                this.triangles.Add(i1); this.triangles.Add(a); this.triangles.Add(c);
                this.triangles.Add(i2); this.triangles.Add(b); this.triangles.Add(a);
                this.triangles.Add(i3); this.triangles.Add(c); this.triangles.Add(b);
                this.triangles.Add(a); this.triangles.Add(b); this.triangles.Add(c); // center triangle
            }
        }
        #endregion Subdivide4 (2x2)

        #region Subdivide9 (3x3)
        private int GetNewVertex9(int i1, int i2, int i3)
        {
            int newIndex = vertices.Count;

            // center points don't go into the edge list
            if (i3 == i1 || i3 == i2)
            {
                uint t1 = ((uint)i1 << 16) | (uint)i2;
                if (newVectices.ContainsKey(t1))
                    return newVectices[t1];
                newVectices.Add(t1, newIndex);
            }

            // calculate new vertex
            vertices.Add((vertices[i1] + vertices[i2] + vertices[i3]) / 3.0f);
            if (normals.Count > 0)
                normals.Add((normals[i1] + normals[i2] + normals[i3]).normalized);
            if (colors.Count > 0)
                colors.Add((colors[i1] + colors[i2] + colors[i3]) / 3.0f);
            if (uv.Count > 0)
                uv.Add((uv[i1] + uv[i2] + uv[i3]) / 3.0f);
            if (uv2.Count > 0)
                uv2.Add((uv2[i1] + uv2[i2] + uv2[i3]) / 3.0f);
            if (uv3.Count > 0)
                uv3.Add((uv3[i1] + uv3[i2] + uv3[i3]) / 3.0f);
            return newIndex;
        }


        /// <summary>
        /// Devides each triangles into 9. A quad(2 tris) will be splitted into 3x3 quads( 18 tris )
        /// </summary>
        /// <param name="mesh"></param>
        public void Subdivide9()
        {
            newVectices = new Dictionary<uint, int>();

            for (int i = 0; i < triangles.Count; i += 3)
            {
                int i1 = triangles[i + 0];
                int i2 = triangles[i + 1];
                int i3 = triangles[i + 2];

                int a1 = GetNewVertex9(i1, i2, i1);
                int a2 = GetNewVertex9(i2, i1, i2);
                int b1 = GetNewVertex9(i2, i3, i2);
                int b2 = GetNewVertex9(i3, i2, i3);
                int c1 = GetNewVertex9(i3, i1, i3);
                int c2 = GetNewVertex9(i1, i3, i1);

                int d = GetNewVertex9(i1, i2, i3);

                this.triangles.Add(i1); this.triangles.Add(a1); this.triangles.Add(c2);
                this.triangles.Add(i2); this.triangles.Add(b1); this.triangles.Add(a2);
                this.triangles.Add(i3); this.triangles.Add(c1); this.triangles.Add(b2);
                this.triangles.Add(d); this.triangles.Add(a1); this.triangles.Add(a2);
                this.triangles.Add(d); this.triangles.Add(b1); this.triangles.Add(b2);
                this.triangles.Add(d); this.triangles.Add(c1); this.triangles.Add(c2);
                this.triangles.Add(d); this.triangles.Add(c2); this.triangles.Add(a1);
                this.triangles.Add(d); this.triangles.Add(a2); this.triangles.Add(b1);
                this.triangles.Add(d); this.triangles.Add(b2); this.triangles.Add(c1);
            }
        }
        #endregion Subdivide9 (3x3)


        #region Subdivide
        /// <summary>
        /// http://wiki.unity3d.com/index.php?title=MeshHelper&oldid=20389
        /// This functions subdivides the mesh based on the level parameter
        /// Note that only the 4 and 9 subdivides are supported so only those divides
        /// are possible. [2,3,4,6,8,9,12,16,18,24,27,32,36,48,64, ...]
        /// The function tried to approximate the desired level 
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="level">Should be a number made up of (2^x * 3^y)
        /// [2,3,4,6,8,9,12,16,18,24,27,32,36,48,64, ...]
        /// </param>
        public void Subdivide(int level)
        {
            if (level < 2)
                return;
            while (level > 1)
            {
                // remove prime factor 3
                while (level % 3 == 0)
                {
                    Subdivide9();
                    level /= 3;
                }
                // remove prime factor 2
                while (level % 2 == 0)
                {
                    Subdivide4();
                    level /= 2;
                }
                // try to approximate. All other primes are increased by one
                // so they can be processed
                if (level > 3)
                    level++;
            }
        }
        #endregion Subdivide

        public void Save(BinaryWriter writer)
        {
            writer.Write(vertices.Count);
            foreach (var item in vertices)
            {
                writer.WriteVector3(item);
            }
            writer.Write(normals.Count);
            foreach (var item in normals)
            {
                writer.WriteVector3(item);
            }
            writer.Write(colors.Count);
            foreach (var item in colors)
            {
                writer.WriteColor(item);
            }
            writer.Write(uv.Count);
            foreach (var item in uv)
            {
                writer.WriteVector2(item);
            }
            writer.Write(uv2.Count);
            foreach (var item in uv2)
            {
                writer.WriteVector2(item);
            }
            writer.Write(uv3.Count);
            foreach (var item in uv3)
            {
                writer.WriteVector2(item);
            }
            writer.Write(triangles.Count);
            foreach (var item in triangles)
            {
                writer.Write(item);
            }
        }

        public void Load(BinaryReader reader)
        {
            int verticesCount = reader.ReadInt32();
            vertices = new List<Vector3>(verticesCount);
            for (int i = 0; i < verticesCount; i++)
            {
                vertices.Add(reader.ReadVector3());
            }
            int normalsCount = reader.ReadInt32();
            normals = new List<Vector3>(normalsCount);
            for (int i = 0; i < normalsCount; i++)
            {
                normals.Add(reader.ReadVector3());
            }
            int colorsCount = reader.ReadInt32();
            colors = new List<Color>(colorsCount);
            for (int i = 0; i < colorsCount; i++)
            {
                colors.Add(reader.ReadColor());
            }
            int uvCount = reader.ReadInt32();
            uv = new List<Vector2>(uvCount);
            for (int i = 0; i < uvCount; i++)
            {
                uv.Add(reader.ReadVector2());
            }
            int uv2Count = reader.ReadInt32();
            uv2 = new List<Vector2>(uv2Count);
            for (int i = 0; i < uv2Count; i++)
            {
                uv2.Add(reader.ReadVector2());
            }
            int uv3Count = reader.ReadInt32();
            uv3 = new List<Vector2>(uv3Count);
            for (int i = 0; i < uv3Count; i++)
            {
                uv3.Add(reader.ReadVector2());
            }
            int triangleCount = reader.ReadInt32();
            triangles = new List<int>(triangleCount);
            for (int i = 0; i < triangleCount; i++)
            {
                triangles.Add(reader.ReadInt32());
            }
        }
    }

}
