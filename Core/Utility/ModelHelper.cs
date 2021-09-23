using NonsensicalKit.Custom;
using System.Collections.Generic;
using UnityEngine;

namespace NonsensicalKit.Utility
{
    public static class ModelHelper
    {
        public static Material GetDiffuseMaterial()
        {
            GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
            primitive.SetActive(false);
            Material diffuse = primitive.GetComponent<MeshRenderer>().sharedMaterial;
            Object.Destroy(primitive);
            return diffuse;
        }

        public static Mesh GetCustomCube(Array3<bool> state, float singleSize)
        {
            MeshBuffer crtMeshBuffer = new MeshBuffer();

            Array4<bool> bool6s = new Array4<bool>(state.length0, state.length1, state.length2, 6);

            for (int i = 0; i < state.length0; i++)
            {
                for (int j = 0; j < state.length1; j++)
                {
                    for (int k = 0; k < state.length2; k++)
                    {
                        if (state[i, j, k] == true)
                        {
                            if (i == 0 || (i > 0 && state[i - 1, j, k] == false))
                            {
                                if (bool6s[i, j, k, 0] == false)
                                {
                                    AddFace(crtMeshBuffer, state, 1, new Int3(i, j, k), bool6s, singleSize);
                                }
                            }

                            if (i == state.length0 - 1 || (i < state.length0 - 1 && state[i + 1, j, k] == false))
                            {
                                if (bool6s[i, j, k, 1] == false)
                                {
                                    AddFace(crtMeshBuffer, state, 2, new Int3(i, j, k), bool6s, singleSize);
                                }
                            }

                            if (j == 0 || (j > 0 && state[i, j - 1, k] == false))
                            {
                                if (bool6s[i, j, k, 2] == false)
                                {
                                    AddFace(crtMeshBuffer, state, 3, new Int3(i, j, k), bool6s, singleSize);
                                }
                            }

                            if (j == state.length1 - 1 || (j < state.length1 - 1 && state[i, j + 1, k] == false))
                            {
                                if (bool6s[i, j, k, 3] == false)
                                {
                                    AddFace(crtMeshBuffer, state, 4, new Int3(i, j, k), bool6s, singleSize);
                                }
                            }

                            if (k == 0 || (k > 0 && state[i, j, k - 1] == false))
                            {
                                if (bool6s[i, j, k, 4] == false)
                                {
                                    AddFace(crtMeshBuffer, state, 5, new Int3(i, j, k), bool6s, singleSize);
                                }
                            }

                            if (k == state.length2 - 1 || (k < state.length2 - 1 && state[i, j, k + 1] == false))
                            {
                                if (bool6s[i, j, k, 5] == false)
                                {
                                    AddFace(crtMeshBuffer, state, 6, new Int3(i, j, k), bool6s, singleSize);
                                }
                            }
                        }
                    }
                }

            }
            return crtMeshBuffer.ToMesh();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="meshBuffer"></param>
        /// <param name="state"></param>
        /// <param name="dir">1到6分别为x负，x正，y负，y正，z负，z正</param>
        /// <param name="crtPoint"></param>
        /// <param name="bool6s"></param>
        private static void AddFace(MeshBuffer meshBuffer, Array3<bool> state, int dir, Int3 crtPoint, Array4<bool> bool6s, float singleSize)
        {
            Int3 dir1;
            Int3 dir2;
            Int3 normal;

            switch (dir)
            {
                case 1:
                    {
                        dir1 = new Int3(0, 1, 0);
                        dir2 = new Int3(0, 0, 1);
                        normal = new Int3(-1, 0, 0);
                    }
                    break;
                case 2:
                    {
                        dir1 = new Int3(0, 1, 0);
                        dir2 = new Int3(0, 0, 1);
                        normal = new Int3(1, 0, 0);
                    }
                    break;
                case 3:
                    {
                        dir1 = new Int3(1, 0, 0);
                        dir2 = new Int3(0, 0, 1);
                        normal = new Int3(0, -1, 0);
                    }
                    break;
                case 4:
                    {
                        dir1 = new Int3(1, 0, 0);
                        dir2 = new Int3(0, 0, 1);
                        normal = new Int3(0, 1, 0);
                    }
                    break;
                case 5:
                    {
                        dir1 = new Int3(1, 0, 0);
                        dir2 = new Int3(0, 1, 0);
                        normal = new Int3(0, 0, -1);
                    }
                    break;
                case 6:
                    {
                        dir1 = new Int3(1, 0, 0);
                        dir2 = new Int3(0, 1, 0);
                        normal = new Int3(0, 0, 1);
                    }
                    break;
                default:
                    return;
            }

            Stack<Int3> points = new Stack<Int3>();

            int minDir1Limit = -1;
            int maxDir1Limit = 2147483647;
            int minDir2Limit = -1;
            int maxDir2Limit = 2147483647;

            points.Push(crtPoint);

            Array3<bool> buffer = new Array3<bool>(state.length0, state.length1, state.length2);
            int arrMax1 = state.length0 - 1;
            int arrMax2 = state.length1 - 1;
            int arrMax3 = state.length2 - 1;

            while (points.Count > 0)
            {
                Int3 point = points.Pop();

                int dir1Value = point.GetValue(dir1);
                int dir2Value = point.GetValue(dir2);

                if (dir1Value < minDir1Limit || dir1Value > maxDir1Limit
                    || dir2Value < minDir2Limit || dir2Value > maxDir2Limit)
                {
                    continue;
                }
                bool6s[point.I1, point.I2, point.I3, dir - 1] = true;
                buffer[point.I1, point.I2, point.I3] = true;

                Int3 dir1Negative = point + (-dir1);
                Int3 dir1Positive = point + dir1;
                Int3 dir2Negative = point + (-dir2);
                Int3 dir2Positive = point + dir2;
                Int3 dir1NegativeFace = dir1Negative + normal;
                Int3 dir1PositiveFace = dir1Positive + normal;
                Int3 dir2NegativeFace = dir2Negative + normal;
                Int3 dir2PositiveFace = dir2Positive + normal;

                if (dir1Negative.CheckBound(arrMax1, arrMax2, arrMax3) == true)
                {
                    if (state[dir1Negative] == true
                     && bool6s[dir1Negative, dir - 1] == false)
                    {
                        if (dir1NegativeFace.CheckBound(arrMax1, arrMax2, arrMax3) == true && state[dir1NegativeFace] == true)
                        {
                            if (dir1Value > minDir1Limit)
                            {
                                minDir1Limit = dir1Value;
                            }
                        }
                        else
                        {
                            points.Push(dir1Negative);
                        }
                    }
                    else if (dir1Value > minDir1Limit && buffer[dir1Negative] == false)
                    {
                        minDir1Limit = dir1Value;
                    }
                }
                else
                {
                    if (dir1Value > minDir1Limit)
                    {
                        minDir1Limit = dir1Value;
                    }
                }

                if (dir1Positive.CheckBound(arrMax1, arrMax2, arrMax3) == true)
                {
                    if (state[dir1Positive] == true
                     && bool6s[dir1Positive, dir - 1] == false)
                    {
                        if (dir1PositiveFace.CheckBound(arrMax1, arrMax2, arrMax3) == true && state[dir1PositiveFace] == true)
                        {
                            if (dir1Value < maxDir1Limit)
                            {
                                maxDir1Limit = dir1Value;
                            }
                        }
                        else
                        {
                            points.Push(dir1Positive);
                        }
                    }
                    else if (dir1Value < maxDir1Limit && buffer[dir1Positive] == false)
                    {
                        maxDir1Limit = dir1Value;
                    }

                }
                else
                {
                    if (dir1Value < maxDir1Limit)
                    {
                        maxDir1Limit = dir1Value;
                    }
                }

                if (dir2Negative.CheckBound(arrMax1, arrMax2, arrMax3) == true)
                {
                    if (state[dir2Negative] == true
                     && bool6s[dir2Negative, dir - 1] == false)
                    {
                        if (dir2NegativeFace.CheckBound(arrMax1, arrMax2, arrMax3) == true && state[dir2NegativeFace] == true)
                        {
                            if (dir2Value > minDir2Limit)
                            {
                                minDir2Limit = dir2Value;
                            }
                        }
                        else
                        {
                            points.Push(dir2Negative);
                        }
                    }
                    else if (dir2Value > minDir2Limit && buffer[dir2Negative] == false)
                    {
                        minDir2Limit = dir2Value;
                    }

                }
                else
                {
                    if (dir2Value > minDir2Limit)
                    {
                        minDir2Limit = dir2Value;
                    }
                }

                if (dir2Positive.CheckBound(arrMax1, arrMax2, arrMax3) == true)
                {
                    if (state[dir2Positive] == true
                     && bool6s[dir2Positive, dir - 1] == false)
                    {
                        if (dir2PositiveFace.CheckBound(arrMax1, arrMax2, arrMax3) == true && state[dir2PositiveFace] == true)
                        {
                            if (dir2Value < maxDir2Limit)
                            {
                                maxDir2Limit = dir2Value;
                            }
                        }
                        else
                        {
                            points.Push(dir2Positive);
                        }
                    }
                    else if (dir2Value < maxDir2Limit && buffer[dir2Positive] == false)
                    {
                        maxDir2Limit = dir2Value;
                    }

                }
                else
                {
                    if (dir2Value < maxDir2Limit)
                    {
                        maxDir2Limit = dir2Value;
                    }
                }
            }

            for (int i = 0; i < state.length0; i++)
            {
                for (int j = 0; j < state.length1; j++)
                {
                    for (int k = 0; k < state.length2; k++)
                    {
                        if (buffer[i, j, k] == true)
                        {
                            Int3 point = new Int3(i, j, k);
                            int dir1Value = point.GetValue(dir1);
                            int dir2Value = point.GetValue(dir2);

                            if (dir1Value < minDir1Limit || dir1Value > maxDir1Limit
                                || dir2Value < minDir2Limit || dir2Value > maxDir2Limit)
                            {
                                bool6s[point.I1, point.I2, point.I3, dir - 1] = false;
                            }
                        }
                    }
                }
            }

            Vector3 normalVector3 = new Vector3(normal.I1, normal.I2, normal.I3);

            Vector3[] point4 = new Vector3[4];
            float step = singleSize;
            float distance = step * 0.5f;
            Vector3 offset = new Vector3((crtPoint.I1 + 0.5f), (crtPoint.I2 + 0.5f), (crtPoint.I3 + 0.5f)) * step;
            Vector3 origin = offset;
            Vector3 faceCenterPoint = origin + normalVector3 * distance;
            Vector3 dir1V3 = new Vector3(dir1.I1, dir1.I2, dir1.I3);
            Vector3 dir2V3 = new Vector3(dir2.I1, dir2.I2, dir2.I3);

            Vector3 dir1MinOffset = (minDir1Limit - crtPoint.GetValue(dir1)) * dir1V3 * step;
            Vector3 dir1MaxOffset = (maxDir1Limit - crtPoint.GetValue(dir1)) * dir1V3 * step;
            Vector3 dir2MinOffset = (minDir2Limit - crtPoint.GetValue(dir2)) * dir2V3 * step;
            Vector3 dir2MaxOffset = (maxDir2Limit - crtPoint.GetValue(dir2)) * dir2V3 * step;

            point4[0] = faceCenterPoint + dir1MinOffset + dir2MinOffset + -dir1V3 * distance + -dir2V3 * distance;
            point4[1] = faceCenterPoint + dir1MaxOffset + dir2MinOffset + dir1V3 * distance + -dir2V3 * distance;
            point4[2] = faceCenterPoint + dir1MaxOffset + dir2MaxOffset + dir1V3 * distance + dir2V3 * distance;
            point4[3] = faceCenterPoint + dir1MinOffset + dir2MaxOffset + -dir1V3 * distance + dir2V3 * distance;

            if (dir == 2 || dir == 3 || dir == 6)
            {
                meshBuffer.AddQuad(point4, normalVector3, Vector2.one * 0.5f);
            }
            else
            {
                meshBuffer.AddQuad(new Vector3[] { point4[2], point4[1], point4[0], point4[3] }, normalVector3, Vector2.one * 0.5f);
            }


            //Vector3 offset2 = new Vector3((granulation.length0*0.5f + 0.5f), (granulation.length1 * 0.5f + 0.5f), (granulation.length2 * 0.5f + 0.5f)) * step;
            //Vector3 origin2 = granulation.origin + offset2;
            //if (dir == 2 || dir == 3 || dir == 6)
            //{
            //    meshBuffer.AddQuad_2(point4, origin2, Vector2.one * 0.5f);
            //}
            //else
            //{
            //    meshBuffer.AddQuad_2(new Vector3[] { point4[2], point4[1], point4[0], point4[3] }, origin2, Vector2.one * 0.5f);
            //}
        }

        public static Mesh GetCustomCubeSimple(Array3<bool> state, float singleSize)
        {
            MeshBuffer meshbuffer = new MeshBuffer();
            int width = state.length0;
            int height = state.length1;
            int thickness = state.length2;

            Array3<bool> temp = new Array3<bool>(width, height, thickness);
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    for (int t = 0; t < thickness; t++)
                    {
                        if (state[w, h, t] == true && temp[w, h, t] == false)
                        {
                            GetPartCube(meshbuffer, state, temp, w, h, t, singleSize);
                        }
                    }

                }
            }

            return meshbuffer.ToMesh();
        }

        private static void GetPartCube(MeshBuffer meshbuffer, Array3<bool> state, Array3<bool> temp, int w, int h, int t, float singleSize)
        {
            int width = state.length0;
            int height = state.length1;
            int thickness = state.length2;

            int left = w;
            int right = left;
            int down = h;
            int up = down;
            int front = t;
            int back = front;

            temp[w, h, t] = true;

            bool wFlag = right + 1 >= width;
            bool hFlag = up + 1 >= height;
            bool tFlag = back + 1 >= thickness;
            while (true)
            {
                if (!wFlag)
                {
                    if (CheckWidth(down, up, front, back, right + 1, state, temp))
                    {
                        wFlag = true;
                    }
                    else
                    {
                        right++;
                        if (right + 1 >= width)
                        {
                            wFlag = true;
                        }
                    }
                }
                if (!hFlag)
                {
                    if (CheckHeight(left, right, front, back, up + 1, state, temp))
                    {
                        hFlag = true;
                    }
                    else
                    {
                        up++;
                        if (up + 1 >= height)
                        {
                            hFlag = true;
                        }
                    }
                }
                if (!tFlag)
                {
                    if (CheckThickness(left, right, down, up, back + 1, state, temp))
                    {
                        tFlag = true;
                    }
                    else
                    {
                        back++;
                        if (back + 1 >= thickness)
                        {
                            tFlag = true;
                        }
                    }
                }
                if (wFlag && hFlag && tFlag)
                {
                    break;
                }
            }

            for (int i = left; i <= right; i++)
            {
                for (int j = down; j <= up; j++)
                {
                    for (int k = front; k <= back; k++)
                    {
                        temp[i, j, k] = true;
                    }
                }
            }

            float leftPos = (left - width * 0.5f) * singleSize;
            float rightPos = (right - width * 0.5f + 1) * singleSize;
            float downPos = down * singleSize;
            float upPos = (up + 1) * singleSize;
            float frontPos = front * singleSize;
            float backPos = (back + 1) * singleSize;

            Vector3 center = new Vector3((leftPos + rightPos) * 0.5f, (downPos + upPos) * 0.5f, (frontPos + backPos) * 0.5f);
            Vector3 size = new Vector3(rightPos - leftPos, upPos - downPos, backPos - frontPos);
            meshbuffer.AddCube(center, size);
        }

        private static bool CheckWidth(int down, int up, int front, int back, int rightPlus, Array3<bool> state, Array3<bool> temp)
        {
            for (int j = down; j <= up; j++)
            {
                for (int k = front; k <= back; k++)
                {
                    if (state[rightPlus, j, k] == false || temp[rightPlus, j, k] == true)
                    {
                        Debug.Log("!!!!!");
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool CheckHeight(int left, int right, int front, int back, int upPlus, Array3<bool> state, Array3<bool> temp)
        {
            for (int j = left; j <= right; j++)
            {
                for (int k = front; k <= back; k++)
                {
                    if (state[j, upPlus, k] == false || temp[j, upPlus, k] == true)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool CheckThickness(int left, int right, int down, int up, int backPlus, Array3<bool> state, Array3<bool> temp)
        {
            for (int j = left; j <= right; j++)
            {
                for (int k = down; k <= up; k++)
                {
                    if (state[j, k, backPlus] == false || temp[j, k, backPlus] == true)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 不平整的地面
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="singleSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static Mesh GetUnevenPlane(Vector3 offset, float singleSize, int count)
        {
            MeshBuffer meshbuffer = new MeshBuffer();
            float half = count * 0.5f;

            Vector3 crtPos;

            crtPos.x = offset.x - half * singleSize;
            crtPos.y = offset.y;
            crtPos.z = offset.z - half * singleSize;

            Vector3 p1Offset = new Vector3(0, 0, singleSize);
            Vector3 p2Offset = new Vector3(singleSize, 0, singleSize);
            Vector3 p3Offset = new Vector3(singleSize, 0, 0);

            int row = 0;
            int column = 0;
            int all = count * count;
            for (int i = 0; i < all; i++)
            {
                float x = row / count;
                float y = column / count;
                meshbuffer.AddQuad(
                    new Vector3[] { PerlinNoise(crtPos), PerlinNoise(crtPos + p1Offset), PerlinNoise(crtPos + p2Offset), PerlinNoise(crtPos + p3Offset) },
                    new Vector3(0, 1, 0),
                    new Vector3(x, y));
                column++;
                crtPos.x += singleSize;
                if (column == count)
                {
                    column = 0;
                    crtPos.x = offset.x - half * singleSize;
                    row++;
                    crtPos.z += singleSize;
                }
            }

            return meshbuffer.ToMesh();
        }

        private static Vector3 PerlinNoise(Vector3 origin)
        {
            origin.y += Mathf.PerlinNoise((origin.x + 100) / 15, (origin.z + 100) / 15);
            return origin;
        }

        public static Mesh GetCube(Vector3 size)
        {
            return GetCube(size.x, size.y, size.z);
        }
        public static Mesh GetCube(Vector3 offset, Vector3 size)
        {
            return GetCube(offset.x, offset.y, offset.z, size.x, size.y, size.z);
        }
        public static Mesh GetCube(float width, float height, float depth)
        {
            MeshBuffer meshbuffer = new MeshBuffer();

            Vector3[] point = new Vector3[8];
            point[0] = new Vector3(-width * 0.5f, -height * 0.5f, -depth * 0.5f);
            point[1] = new Vector3(-width * 0.5f, height * 0.5f, -depth * 0.5f);
            point[2] = new Vector3(width * 0.5f, height * 0.5f, -depth * 0.5f);
            point[3] = new Vector3(width * 0.5f, -height * 0.5f, -depth * 0.5f);
            point[4] = new Vector3(-width * 0.5f, -height * 0.5f, depth * 0.5f);
            point[5] = new Vector3(-width * 0.5f, height * 0.5f, depth * 0.5f);
            point[6] = new Vector3(width * 0.5f, height * 0.5f, depth * 0.5f);
            point[7] = new Vector3(width * 0.5f, -height * 0.5f, depth * 0.5f);

            //init
            Vector2 middle = Vector2.one * 0.5f;

            //front
            meshbuffer.AddQuad(new Vector3[] { point[0], point[1], point[2], point[3] }, new Vector3(0, 0, -1), middle);

            //back
            meshbuffer.AddQuad(new Vector3[] { point[7], point[6], point[5], point[4] }, new Vector3(0, 0, 1), middle);

            //left
            meshbuffer.AddQuad(new Vector3[] { point[4], point[5], point[1], point[0] }, new Vector3(-1, 0, 0), middle);

            //right
            meshbuffer.AddQuad(new Vector3[] { point[3], point[2], point[6], point[7] }, new Vector3(1, 0, 0), middle);

            //down
            meshbuffer.AddQuad(new Vector3[] { point[0], point[3], point[7], point[4] }, new Vector3(0, -1, 0), middle);

            //up
            meshbuffer.AddQuad(new Vector3[] { point[1], point[5], point[6], point[2] }, new Vector3(0, 1, 0), middle);


            return meshbuffer.ToMesh();
        }
        public static Mesh GetCube(float offsetX, float offsetY, float offsetZ, float width, float height, float depth)
        {
            MeshBuffer meshbuffer = new MeshBuffer();
            Vector3 offset = new Vector3(offsetX, offsetY, offsetZ);
            Vector3[] point = new Vector3[8];
            point[0] = offset + new Vector3(-width * 0.5f, -height * 0.5f, -depth * 0.5f);
            point[1] = offset + new Vector3(-width * 0.5f, height * 0.5f, -depth * 0.5f);
            point[2] = offset + new Vector3(width * 0.5f, height * 0.5f, -depth * 0.5f);
            point[3] = offset + new Vector3(width * 0.5f, -height * 0.5f, -depth * 0.5f);
            point[4] = offset + new Vector3(-width * 0.5f, -height * 0.5f, depth * 0.5f);
            point[5] = offset + new Vector3(-width * 0.5f, height * 0.5f, depth * 0.5f);
            point[6] = offset + new Vector3(width * 0.5f, height * 0.5f, depth * 0.5f);
            point[7] = offset + new Vector3(width * 0.5f, -height * 0.5f, depth * 0.5f);

            //init
            Vector2 middle = Vector2.one * 0.5f;

            //front
            meshbuffer.AddQuad(new Vector3[] { point[0], point[1], point[2], point[3] }, new Vector3(0, 0, -1), middle);

            //back
            meshbuffer.AddQuad(new Vector3[] { point[7], point[6], point[5], point[4] }, new Vector3(0, 0, 1), middle);

            //left
            meshbuffer.AddQuad(new Vector3[] { point[4], point[5], point[1], point[0] }, new Vector3(-1, 0, 0), middle);

            //right
            meshbuffer.AddQuad(new Vector3[] { point[3], point[2], point[6], point[7] }, new Vector3(1, 0, 0), middle);

            //down
            meshbuffer.AddQuad(new Vector3[] { point[0], point[3], point[7], point[4] }, new Vector3(0, -1, 0), middle);

            //up
            meshbuffer.AddQuad(new Vector3[] { point[1], point[5], point[6], point[2] }, new Vector3(0, 1, 0), middle);


            return meshbuffer.ToMesh();
        }
        public static Mesh GetCylinder(float radius, float height)
        {
            MeshBuffer meshbuffer = new MeshBuffer();

            meshbuffer.AddRing(Vector3.zero, 0, radius, Vector3.up, 32);
            meshbuffer.AddRing3D(Vector3.zero, radius, new Vector3(0, height, 0), radius, Vector3.up, 32);
            meshbuffer.AddRing(new Vector3(0, height, 0), 0, radius, -Vector3.up, 32);

            return meshbuffer.ToMesh();
        }
    }

}

