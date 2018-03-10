#pragma once
class CTask
{
	int* m_nCount;
public:
	CTask(int* nCount);
	~CTask();

	void DoWork();
};