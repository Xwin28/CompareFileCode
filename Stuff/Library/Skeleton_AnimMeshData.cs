﻿using ECS_AnimationSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_AnimMeshData : MonoBehaviour
{
    public Mesh mesh;
    public Vector3[] vertices;
    private int verticesLength;
    public Vector2[] uv;
    public int[] triangles;
    public Quad[] quadArray;
    public int quadIgnoreIndex;

    public Skeleton_AnimMeshData(int quadArrayLength)
    {
        mesh = new Mesh();
        mesh.MarkDynamic();
        //quadArrayLength = 7;
        quadArray = new Quad[quadArrayLength];
        vertices = new Vector3[4 * quadArrayLength];
        verticesLength = vertices.Length;
        uv = new Vector2[4 * quadArrayLength];

        triangles = new int[6 * quadArrayLength];// { 0, 1, 2, 2, 1, 3,     4+0, 4+1, 4+2, 4+2, 4+1, 4+3 };
        for (int i = 0; i < quadArrayLength; i++)
        {
            triangles[(i * 6) + 0] = (i * 4) + 0;
            triangles[(i * 6) + 1] = (i * 4) + 1;
            triangles[(i * 6) + 2] = (i * 4) + 2;
            triangles[(i * 6) + 3] = (i * 4) + 2;
            triangles[(i * 6) + 4] = (i * 4) + 1;
            triangles[(i * 6) + 5] = (i * 4) + 3;
        }

        quadIgnoreIndex = 0;
    }

    public void SetIgnoreQuad(int index)
    {
        quadIgnoreIndex = index;
    }

    public void SetQuad(int index, Quad quad)
    {
        quadArray[index] = quad;
    }

    private int verticeIndex;
    public void RefreshMesh()
    {
        verticeIndex = 0;

        Vector3 verticesZero = vertices[0];

        for (int i = 0; i < quadIgnoreIndex; i++)
        {
            Quad quad = quadArray[i];

            // Use Quad
            QuadVertices quadVertices = quad.quadVertices;
            vertices[verticeIndex] = new Vector3(quadVertices.v01.x, quadVertices.v01.y);
            vertices[verticeIndex + 1] = new Vector3(quadVertices.v11.x, quadVertices.v11.y);
            vertices[verticeIndex + 2] = new Vector3(quadVertices.v00.x, quadVertices.v00.y);
            vertices[verticeIndex + 3] = new Vector3(quadVertices.v10.x, quadVertices.v10.y);

            QuadUV quadUV = quad.quadUV;
            uv[verticeIndex] = new Vector2(quadUV.uv00.x, quadUV.uv11.y);
            uv[verticeIndex + 1] = new Vector2(quadUV.uv11.x, quadUV.uv11.y);
            uv[verticeIndex + 2] = new Vector2(quadUV.uv00.x, quadUV.uv00.y);
            uv[verticeIndex + 3] = new Vector2(quadUV.uv11.x, quadUV.uv00.y);

            verticeIndex = verticeIndex + 4;
        }
        while (verticeIndex < verticesLength)
        {
            vertices[verticeIndex] = verticesZero;
            verticeIndex++;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
}
