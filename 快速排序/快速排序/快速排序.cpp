// 快速排序.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"

#include<iostream>
#include<stdlib.h>
#include<time.h> 
#define MIN 0    //随机数产生的范围      
#define MAX 60000
using namespace std;
void quiksort(int a[],int low,int high)
{
	int i=low;
	int j=high;
	int temp=a[low];
	while(i<j)
	{
		//右侧扫描
		while(i<j&&temp<=a[j])j--;
		if(i<j)
		{
			a[i]=a[j];
			i++;
		}
		//左侧
		while(i<j&&temp>a[i])i++;
		if(i<j)
		{
			a[j]=a[i];
			j--;
		}
		a[i]=temp;
		if(i>low)quiksort(a,low,i-1);
		if(i<high)quiksort(a,j+1,high);
	}
}
int _tmain(int argc, _TCHAR* argv[])
{
	int i;
	int a[1000];
	srand((unsigned)time(NULL));  
	cout<<"Ten random numbers from "<<MIN<<  
		" to "<<MAX<<" :/n"<<endl;     
	for(i=0; i<1000; i++)          //产生随机数  
	{  
		a[i]=MIN + rand() % (MAX + MIN - 1);

		if(i%10==0)cout<<endl;
		cout<<a[i]<<"  ";
	}  
	quiksort(a,0,999);
	cout<<"排序后:"<<endl;
	for(i=0; i<1000; i++)         
	{  


		if(i%10==0)cout<<endl;
		cout<<a[i]<<"  ";
	}  

	return 0;
}

