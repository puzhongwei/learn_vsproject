// threadTest.cpp : �������̨Ӧ�ó������ڵ㡣
//

#include "stdafx.h"
#pragma comment(lib, "pthreadVC2.lib")  //����������
/*#include<stdio.h>
#include<pthread.h>
#include<Windows.h>


void*Function_t(void* Param)
{
	int i=*(int *)Param;
     pthread_t myid = pthread_self();
	  
     while(i<5)
     {
         printf("�߳�ID=%d \n", myid);
		 i++;
		 printf("%d",i);
         Sleep(4000);
     }
     return NULL;
}
 


int _tmain(int argc, _TCHAR* argv[])
{
	
     pthread_t pid[100];
	 int k[5]={0};
	 for(int i=0;i<10;i++){
     pthread_create(&pid[i], NULL, Function_t,&k[i]);
     
    
         //printf("in fatherprocess!\n");
         Sleep(2000);
	 }
    // getchar();
    
	return 0;
}*/

/*#include <iostream>
#include <pthread.h>

using namespace std;

#define NUM_THREADS 5

class Hello
{
public:
    static void* say_hello( void* args )
    {
		//int i =*((int *)args);
		//for(i;i<5;i++){
		pthread_t myid = pthread_self();
		printf("�߳�ID=%d:  ", myid);
		//}
        //cout << "hello..." << endl;
		return NULL;
    }
	;
};

int  _tmain(int argc, _TCHAR* argv[])
{
    pthread_t tids[NUM_THREADS];
	int k=0;
    for( int i = 0; i < NUM_THREADS; ++i )
    {
        int ret = pthread_create( &tids[i], NULL, Hello::say_hello,NULL );
        if( ret != 0 )
        {
            cout << "pthread_create error:error_code" << ret << endl;
        }
    }
    pthread_exit( NULL );
}*/
/*#include <iostream>
#include <pthread.h>

using namespace std;

#define NUM_THREADS 5

void* say_hello( void* args )
{
    cout << "hello in thread " << *(( int * )args) << endl;
    int status = 10 + *(( int * )args); //�߳��˳�ʱ����˳�����Ϣ��status����������ȡ���̵߳Ľ�����Ϣ
    pthread_exit( ( void* )status ); 
	return NULL;
}

int _tmain(int argc, _TCHAR* argv[])
{
    pthread_t tids[NUM_THREADS];
    int indexes[NUM_THREADS];
    
    pthread_attr_t attr; //�߳����Խṹ�壬�����߳�ʱ����Ĳ���
    pthread_attr_init( &attr ); //��ʼ��
    pthread_attr_setdetachstate( &attr, PTHREAD_CREATE_JOINABLE ); //����������Ҫָ���߳����Բ��������������������߳��ǿ���join���ӵģ�join���ܱ�ʾ��������Ե��߳̽�������ȥ��ĳ�£�ʵ������������߳�ͬ������
    for( int i = 0; i < NUM_THREADS; ++i )
    {
        indexes[i] = i;
        int ret = pthread_create( &tids[i], &attr, say_hello, ( void* )&( indexes[i] ) );
        if( ret != 0 )
        {
	    cout << "pthread_create error:error_code=" << ret << endl;
	}
    } 
    pthread_attr_destroy( &attr ); //�ͷ��ڴ� 
    void *status;
    for( int i = 0; i < NUM_THREADS; ++i )
    {
	int ret = pthread_join( tids[i], &status ); //������joinÿ���̺߳�ȡ��ÿ���̵߳��˳���Ϣstatus
	if( ret != 0 )
	{
	    cout << "pthread_join error:error_code=" << ret << endl;
	}
	else
	{
	    cout << "pthread_join get status:" << (long)status << endl;
	}
    }
}*/
#include <iostream>
#include <pthread.h>

using namespace std;

#define NUM_THREADS 5

int sum = 0; //����ȫ�ֱ������������߳�ͬʱд����������Ҫ������
pthread_mutex_t sum_mutex; //������

void* say_hello( void* args )
{
    cout << "hello in thread " << *(( int * )args) << endl;
    pthread_mutex_lock( &sum_mutex ); //�ȼ��������޸�sum��ֵ������ռ�þ�������ֱ���õ������޸�sum;
    cout << "before sum is " << sum << " in thread " << *( ( int* )args ) << endl;
    sum += *( ( int* )args );
    cout << "after sum is " << sum << " in thread " << *( ( int* )args ) << endl;
    pthread_mutex_unlock( &sum_mutex ); //�ͷ������������߳�ʹ��
    pthread_exit( 0 ); 
	return NULL;
}

int _tmain(int argc, _TCHAR* argv[])
{
    pthread_t tids[NUM_THREADS];
    int indexes[NUM_THREADS];
    
    pthread_attr_t attr; //�߳����Խṹ�壬�����߳�ʱ����Ĳ���
    pthread_attr_init( &attr ); //��ʼ��
    pthread_attr_setdetachstate( &attr, PTHREAD_CREATE_JOINABLE ); //����������Ҫָ���߳����Բ��������������������߳��ǿ���join���ӵģ�join���ܱ�ʾ��������Ե��߳̽�������ȥ��ĳ�£�ʵ������������߳�ͬ������
    pthread_mutex_init( &sum_mutex, NULL ); //�������г�ʼ��    

    for( int i = 0; i < NUM_THREADS; ++i )
    {
        indexes[i] = i;
        int ret = pthread_create( &tids[i], &attr, say_hello, ( void* )&( indexes[i] ) ); //5������ͬʱȥ�޸�sum
        if( ret != 0 )
        {
	    cout << "pthread_create error:error_code=" << ret << endl;
	}
    } 
    pthread_attr_destroy( &attr ); //�ͷ��ڴ� 
    void *status;
    for( int i = 0; i < NUM_THREADS; ++i )
    {
	int ret = pthread_join( tids[i], &status ); //������joinÿ���̺߳�ȡ��ÿ���̵߳��˳���Ϣstatus
	if( ret != 0 )
	{
	    cout << "pthread_join error:error_code=" << ret << endl;
	}
    }
    cout << "finally sum is " << sum << endl;
    pthread_mutex_destroy( &sum_mutex ); //ע����
}
