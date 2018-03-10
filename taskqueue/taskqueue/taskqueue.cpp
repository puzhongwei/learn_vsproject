// taskqueue.cpp : �������̨Ӧ�ó������ڵ㡣
//

#include "stdafx.h"
#include "windows.h"
#include "Task.h"
#include <queue>
#include <mutex>
#include <thread>

class CTaskQueue
{
public:
	CTaskQueue();
	~CTaskQueue();

private:
	std::queue<CTask*> m_taskQueue;  //�������
	std::thread m_thread;
	std::mutex m_mutex;
	bool m_bIsStart;   //�߳��Ƿ���

public:
	//�����߳�
	void WorkThread();

	//����������������
	bool Push(CTask* task);
	//��������л�ȡ����
	CTask* Pop();

	//�����߳�
	bool Start();
	//��ֹ�߳�
	void Stop();
};

CTaskQueue::CTaskQueue()
{
}


CTaskQueue::~CTaskQueue()
{
}


//�����߳�
void CTaskQueue::WorkThread()
{
	while (m_bIsStart)
	{
		if (!m_taskQueue.empty())
		{
			CTask* it = m_taskQueue.front();
			it->DoWork();
			m_taskQueue.pop();
			delete it;
		}
	}
}

//����������������
bool CTaskQueue::Push(CTask* task)
{
	if (task == nullptr)
	{
		return false;
	}

	m_mutex.lock();
	m_taskQueue.push(task);
	m_mutex.unlock();

	return true;
}
//��������л�ȡ����
CTask* CTaskQueue::Pop()
{
	CTask* it = nullptr;

	m_mutex.lock();
	if (!m_taskQueue.empty())
	{
		it = m_taskQueue.front();
		m_taskQueue.pop();
	}
	m_mutex.unlock();
	return it;
}

bool CTaskQueue::Start()
{
	/*if (m_bIsStart)
	{
		return false;
	}*/
	m_bIsStart = true;
	m_thread = std::thread(&CTaskQueue::WorkThread, this);
	m_thread.join();
	return true;
}

void CTaskQueue::Stop()
{
	m_bIsStart = false;
	m_thread.detach();
}


void MyWorkTask1(CTaskQueue* pTaskQueue, int* nCount)
{
	for (size_t i = 0; i < 20; i++)
	{
		CTask* task = new CTask(nCount);
		pTaskQueue->Push(task);
	}
}

void MyWorkTask2(CTaskQueue* pTaskQueue, int* nCount)
{
	for (size_t i = 0; i < 20; i++)
	{
		CTask* task = new CTask(nCount);
		pTaskQueue->Push(task);
	}
}

int _tmain(int argc, _TCHAR* argv[])
{
	CTaskQueue* pTaskQueue = new CTaskQueue();
	
	int* nCount = new  int(0);
	for (size_t i = 0; i < 20; i++)
	{
		CTask* task = new CTask(nCount);
		pTaskQueue->Push(task);
	}
	pTaskQueue->Start();
	pTaskQueue->Stop();
	

	/*int* nCount = new  int(0);

	std::thread thread1(&MyWorkTask1, pTaskQueue, nCount);
	std::thread thread2(&MyWorkTask2, pTaskQueue, nCount);

	�ȴ��߳̽���
	if (thread1.joinable())
	{
		thread1.join();
	}
	if (thread2.joinable())
	{
		thread2.join();
	}
	
	system("pause");*/
	return 0;
}

