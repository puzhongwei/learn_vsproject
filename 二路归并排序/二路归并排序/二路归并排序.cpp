// ��·�鲢����.cpp : �������̨Ӧ�ó������ڵ㡣
//

#include "stdafx.h"
#include"iostream"
#include<stdlib.h>
#include<time.h> 
#define MIN 0    //����������ķ�Χ      
#define MAX 60000
using namespace std;

void Merge(int a[],int n,int swap[],int k)
{
	int m=0,u1,l2,i,j,u2;
	int l1=0;
	while(l1+k<=n-1)
	{
		l2=l1+k;
		u2=l2-1;
		u2=(l2+k-1<=n-1)?l2+k-1:n-1;

		//������������ϲ�
		for(i=l1,j=l2;i<u1&&j<u2;m++)
		{
			if(a[i]<=a[j])
			{
				swap[m]=a[i];
				i++;
			}
			else
			{
				swap[m]=a[j];
				j++;
			}
		}
		//������2�Ѿ��鲢��ϣ���1��ʣ��Ԫ�ط�������swap��
		while(i<u1)
		{
			swap[m]=a[i];
			m++;
			i++;
		}
		//������1�Ѿ��鲢��ϣ���2��ʣ��Ԫ�ط���swap��
		while(j<u2)
		{
			swap[m]=a[j];
			m++;
			j++;
		}
		l1=u2+1;
	}
	for(i=l1;i<n;i++,m++)
		swap[m]=a[i];
}

void Mergesort(int a[],int n)
{
	int i,k=1;
	int *swap=new int [n];

	while(k<n)
	{
		Merge(a,n,swap,k);
		for(i=0;i<n;i++)
			a[i]=swap[i];
		k=k*2;
	}
	delete []swap;
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
	//quiksort(a,0,999);
	Mergesort(a,1000);
	cout<<"�����:"<<endl;
	 for(i=0; i<1000; i++)         
    {  
       
		
		if(i%10==0)cout<<endl;
		cout<<a[i]<<"  ";
    }  
	return 0;
}

