// threadTest.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#pragma comment(lib, "pthreadVC2.lib")  //必须加上这句
/*#include<stdio.h>
#include<pthread.h>
#include<Windows.h>


void*Function_t(void* Param)
{
	int i=*(int *)Param;
     pthread_t myid = pthread_self();
	  
     while(i<5)
     {
         printf("线程ID=%d \n", myid);
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
		printf("线程ID=%d:  ", myid);
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
    int status = 10 + *(( int * )args); //线程退出时添加退出的信息，status供主程序提取该线程的结束信息
    pthread_exit( ( void* )status ); 
	return NULL;
}

int _tmain(int argc, _TCHAR* argv[])
{
    pthread_t tids[NUM_THREADS];
    int indexes[NUM_THREADS];
    
    pthread_attr_t attr; //线程属性结构体，创建线程时加入的参数
    pthread_attr_init( &attr ); //初始化
    pthread_attr_setdetachstate( &attr, PTHREAD_CREATE_JOINABLE ); //是设置你想要指定线程属性参数，这个参数表明这个线程是可以join连接的，join功能表示主程序可以等线程结束后再去做某事，实现了主程序和线程同步功能
    for( int i = 0; i < NUM_THREADS; ++i )
    {
        indexes[i] = i;
        int ret = pthread_create( &tids[i], &attr, say_hello, ( void* )&( indexes[i] ) );
        if( ret != 0 )
        {
	    cout << "pthread_create error:error_code=" << ret << endl;
	}
    } 
    pthread_attr_destroy( &attr ); //释放内存 
    void *status;
    for( int i = 0; i < NUM_THREADS; ++i )
    {
	int ret = pthread_join( tids[i], &status ); //主程序join每个线程后取得每个线程的退出信息status
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

int sum = 0; //定义全局变量，让所有线程同时写，这样就需要锁机制
pthread_mutex_t sum_mutex; //互斥锁

void* say_hello( void* args )
{
    cout << "hello in thread " << *(( int * )args) << endl;
    pthread_mutex_lock( &sum_mutex ); //先加锁，再修改sum的值，锁被占用就阻塞，直到拿到锁再修改sum;
    cout << "before sum is " << sum << " in thread " << *( ( int* )args ) << endl;
    sum += *( ( int* )args );
    cout << "after sum is " << sum << " in thread " << *( ( int* )args ) << endl;
    pthread_mutex_unlock( &sum_mutex ); //释放锁，供其他线程使用
    pthread_exit( 0 ); 
	return NULL;
}

int _tmain(int argc, _TCHAR* argv[])
{
    pthread_t tids[NUM_THREADS];
    int indexes[NUM_THREADS];
    
    pthread_attr_t attr; //线程属性结构体，创建线程时加入的参数
    pthread_attr_init( &attr ); //初始化
    pthread_attr_setdetachstate( &attr, PTHREAD_CREATE_JOINABLE ); //是设置你想要指定线程属性参数，这个参数表明这个线程是可以join连接的，join功能表示主程序可以等线程结束后再去做某事，实现了主程序和线程同步功能
    pthread_mutex_init( &sum_mutex, NULL ); //对锁进行初始化    

    for( int i = 0; i < NUM_THREADS; ++i )
    {
        indexes[i] = i;
        int ret = pthread_create( &tids[i], &attr, say_hello, ( void* )&( indexes[i] ) ); //5个进程同时去修改sum
        if( ret != 0 )
        {
	    cout << "pthread_create error:error_code=" << ret << endl;
	}
    } 
    pthread_attr_destroy( &attr ); //释放内存 
    void *status;
    for( int i = 0; i < NUM_THREADS; ++i )
    {
	int ret = pthread_join( tids[i], &status ); //主程序join每个线程后取得每个线程的退出信息status
	if( ret != 0 )
	{
	    cout << "pthread_join error:error_code=" << ret << endl;
	}
    }
    cout << "finally sum is " << sum << endl;
    pthread_mutex_destroy( &sum_mutex ); //注销锁
}
