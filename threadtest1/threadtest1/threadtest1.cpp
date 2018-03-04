// threadtest1.cpp : �������̨Ӧ�ó������ڵ㡣
//

#include "stdafx.h"
#include <iostream>
#include <pthread.h> //���߳���ز���ͷ�ļ�������ֲ�ڶ�ƽ̨

using namespace std;

#define NUM_THREADS 5 //�߳���

void* say_hello( void* args )
{
    cout << "hello..." << endl;
	return NULL;
} //�������ص��Ǻ���ָ�룬���ں�����Ϊ����

int main()
{
    pthread_t tids[NUM_THREADS]; //�߳�id
    for( int i = 0; i < NUM_THREADS; ++i )
    {
        int ret = pthread_create( &tids[i], NULL, say_hello, NULL ); //�������������߳�id���̲߳������߳����к�������ʼ��ַ�����к����Ĳ���
        if( ret != 0 ) //�����̳߳ɹ�����0
        {
            cout << "pthread_create error:error_code=" << ret << endl;
        }
    }
    pthread_exit( NULL ); //�ȴ������߳��˳��󣬽��̲Ž������������ǿ�ƽ������̴߳���δ��ֹ��״̬
}


int _tmain(int argc, _TCHAR* argv[])
{
	return 0;
}

