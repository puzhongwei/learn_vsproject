#include "stdafx.h"
#include "Task.h"
#include <iostream>
using namespace std;

CTask::CTask(int* nCount)
{
	m_nCount = nCount;
}

CTask::~CTask()
{
}

void CTask::DoWork()
{
	(*m_nCount)++;

	cout << "Count = " << *m_nCount << endl;
}