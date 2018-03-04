// ��������.cpp : �������̨Ӧ�ó������ڵ㡣
//

#include "stdafx.h"

#include<iostream>
#include<stdlib.h>
#include<time.h> 
#define MIN 0    //����������ķ�Χ      
#define MAX 60000
using namespace std;
void quiksort(int a[],int low,int high)
{
	int i=low;
	int j=high;
	int temp=a[low];
	while(i<j)
	{
		//�Ҳ�ɨ��
		while(i<j&&temp<=a[j])j--;
		if(i<j)
		{
			a[i]=a[j];
			i++;
		}
		//���
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
	for(i=0; i<1000; i++)          //���������  
	{  
		a[i]=MIN + rand() % (MAX + MIN - 1);

		if(i%10==0)cout<<endl;
		cout<<a[i]<<"  ";
	}  
	quiksort(a,0,999);
	cout<<"�����:"<<endl;
	for(i=0; i<1000; i++)         
	{  


		if(i%10==0)cout<<endl;
		cout<<a[i]<<"  ";
	}  

	return 0;
}

