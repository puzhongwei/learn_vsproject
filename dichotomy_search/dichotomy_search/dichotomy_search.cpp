// dichotomy_search.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include"stdio.h"
#include"iostream"
using namespace std;
#define MAXSIZE 10
typedef struct{
	int list[MAXSIZE];
	int length;

}List;
int dichotomy_search(List s,int k)
{
	int low,mid,high;
	low=0;
	high=s.length-1;
	mid=(low+high)/2;
	while(high>=low)
	{
		//cout<<mid<<endl;

	if(s.list[mid]>k)
	{
		high=mid-1;
		mid=(low+high)/2;
	}
	else if(s.list[mid]<k)
	{
		low=mid+1;
		mid=(low+high)/2;

     }
	else 
		return (mid+1);
	}
	return 0;
}

int _tmain(int argc, _TCHAR* argv[])
{
	List s;
	int i,k,rst;
	int a[MAXSIZE]={1,3,6,12,15,19,25,32,38,87};
	for(i=0;i<MAXSIZE;i++)
	{
		s.list[i]=a[i];
	}
	s.length=MAXSIZE;
	printf("Input key number：");
		scanf("%d",&k);
	rst=dichotomy_search(s,k);
	if(rst==0)
	{
		printf("Key:%d is not in the list\n",k);
	}
	else printf("The key is  in the list,position is：%d\n",rst);
	return 0;
}

