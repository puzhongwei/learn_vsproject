// digital.cpp : �������̨Ӧ�ó������ڵ㡣
//

#include "stdafx.h"
#include<stdio.h>
#include<cmath>
#include<iostream>
using namespace std;   
    int _tmain(int argc, _TCHAR* argv[])  
    {  
        long n;  
        int p,c,m=0,s[100];  
        cout<<"����Ҫת�������֣�"<<endl;  
        cin>>n;  
        cout<<"����Ҫת���Ľ��ƣ�"<<endl;  
        cin>>p;  
      
        cout<<"("<<n<<")10="<<"(";  
      
        while (n!=0)//����ת���������������s[m]  
        {  
            c=n%p;  
            n=n/p;  
            m++;s[m]=c;   //��������˳���������s[m]��  
        }  
      
        for(int k=m;k>=1;k--)//���ת���������  
        {  
            if(s[k]>=10) //��Ϊʮ�����Ƶ���������Ӧ����ĸ  
                cout<<(char)(s[k]+55);  
            else         //����ֱ���������  
                cout<<s[k];  
        }  
      
        cout<<")"<<p<<endl;  
      
        return 0;  
    }  