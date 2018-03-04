// smartPoint.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include<iostream>

int row,col;
int matrix[100][100]={0};
void UpFillNum(int);
void DownFillNum(int);
int _tmain(int argc, _TCHAR* argv[])
{
  int n;
  int i,j;  
  printf("请输入矩阵的阶数:（0<n<=100）");
  scanf("%d",&n);
  matrix[0][0]=1;
  matrix[1][0]=2;
  row=1;
  col=0;
  UpFillNum(n-1);
  for(i=0;i<n;i++)
  {
	for(j=0;j<n;j++)
	{
	printf("%d ",matrix[i][j]);
	}
	printf("\n");
  }
  return 0;
}

//从上向下填充
void DownFillNum(int n)
{
  for(;col>0 && row<n;row++,col--)
	matrix[row+1][col-1]=matrix[row][col]+1;
  if(row<n)
  {
	row++;
	matrix[row][col]=matrix[row-1][col]+1;
  }
  else if(row==n)
  {
	col++;
	matrix[row][col]=matrix[row][col-1]+1;
  }
  if(matrix[n][n]!=((n+1)*(n+1)))
	UpFillNum(n);
}

//从下向上填充
void UpFillNum(int n)
{
  for(;row>0 && col<n;col++,row--)
	matrix[row-1][col+1]=matrix[row][col]+1;
  if(col<n)
  {
	col++;
	matrix[row][col]=matrix[row][col-1]+1;
  }
  else if(col==n)
  {
	row++;
	matrix[row][col]=matrix[row-1][col]+1;
  }
  if(matrix[n][n]!=((n+1)*(n+1)))
	DownFillNum(n);
}



