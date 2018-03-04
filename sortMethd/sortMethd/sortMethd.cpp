
// sortMethd.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include"iostream"
using namespace std;
void printr(int a[],int n)
{
	for (int i=0;i<n;i++)
	{
		cout<<a[i]<<"  ";
	}
	cout<<endl;
}
void InsertSort(int a[],int n)
{
 for (int j=1; j<n; j++)  
    {  
        int key = a[j];  
        int i = j-1;  
        while (i>=0 && a[i]>key)  
        {  
            a[i+1] = a[i];  
            i--;  
        }  
        a[i+1] = key;  
    }  
	printr(a,n);
}

//shell
void ShellInsertSort(int a[],int n,int d )
{
	int j=0;
     int x=0;
	for (int i=0;i<n;i++)
	{
		if(a[i]<a[i-d])
		{
			j=i-d;
		    x=a[i];
			a[i]=a[j];
		}
		while(x<a[j])
		{
			a[j+d]=a[j];
			j-=d;
		}
		a[j+d]=x;
	}
	printr(a,8);
}
int _tmain(int argc, _TCHAR* argv[])
{
	int a[8]={3,1,5,7,2,4,9,6};
	//InsertSort(a,8);
	ShellInsertSort(a,8,1 );
	return 0;
}

