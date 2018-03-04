// �ؽ�������.cpp : �������̨Ӧ�ó������ڵ㡣
//

#include "stdafx.h"

#include<iostream>

using namespace std;

struct BinaryTreeNode
{
	int m_nValue;
	BinaryTreeNode* m_pLeft;
	BinaryTreeNode* m_pRight;
};

int _tmain(int argc, _TCHAR* argv[])
{
	return 0;
}
BinaryTreeNode* ConstructCore
	(
	int *startPreorder,int * endPreorder,
	int *startInorder,int *endInorder
	)
{
	int rootValue=startPreorder[0];
	BinaryTreeNode* root=new BinaryTreeNode();
	root->m_nValue=rootValue;
	root->m_pLeft=root->m_pRight=NULL;

	if(startPreorder==endPreorder)
	{
		if(startInorder==endInorder&&*startPreorder==*endPreorder)
			return root;
		else
			throw std::exception("Invalid input.");

	}
	//�����������Ѱ�Ҹ��ڵ�
	int *rootInorder=startPreorder;
	while(rootInorder<=endInorder&&*rootInorder!=rootValue)
		++ rootInorder;

	if(rootInorder==endInorder&&*rootInorder!=rootValue)
		throw std::exception("Invalid input.");
	int leftLength=rootInorder-startInorder;
	int *leftPreorderEnd=startPreorder+leftLength;

	if(leftLength>0)
	{
		//����������
		root->m_pLeft=ConstructCore(startPreorder+1,leftPreorderEnd,startInorder,rootInorder-1);

	}
	if(leftLength<endPreorder-startPreorder)
	{
		//����������
		root->m_pRight=ConstructCore(leftPreorderEnd+1,endPreorder,rootInorder+1,endInorder);

	}

	return root;
}
BinaryTreeNode* Construct(int * preorder,int *inorder,int length)
{
	if(preorder==NULL||inorder==NULL||length<=0)
		return NULL;

	return ConstructCore(preorder,preorder+length-1,inorder,inorder+length-1);
}

