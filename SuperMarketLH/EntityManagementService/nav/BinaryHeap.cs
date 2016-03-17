using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace EntityManagementService.nav
{
    public class BinaryHeap
    {
        private Node[] heap = new Node[1400 * 540];
        private int heap_size = 0;
        public int getHeapSize()
        {
            return this.heap_size;
        }
        public Node[] getHeap()
        {
            return this.heap;
        }
        public int getLeft(int i)
        {
            return 2 * (i + 1) - 1;
        }
        public int getRight(int i)
        {
            return 2 * (i + 1);
        }
        //查找一个节点
        public int isInHeap(Point p)
        {
            for (int i = 0; i < heap_size; i++)
            {
                if (heap[i].P.X == p.X && heap[i].P.Y == p.Y)
                {
                    return i;
                }
            }
            return -1;
        }


        //删除一个节点
        public void remove(int i)
        {
            if (heap_size > 0)
            {
                Node temp = heap[i];
                heap[i] = heap[heap_size - 1];
                heap[heap_size - 1] = temp;
                heap_size--;
                Min_heap(i);
            }
        }
        //插入一个节点
        public void insert(Node node)
        {
            if (heap_size < 1400 * 540)
            {
                heap_size++;
                heap[heap_size - 1] = node;
                //与父节点递归比较
                //若是小于父节点，则交换

                int current_index = heap_size - 1;
                int parent = (current_index - 1) / 2;
                while (current_index > 0)
                {
                    if (heap[parent].F > heap[current_index].F)
                    {
                        Node temp = heap[current_index];
                        heap[current_index] = heap[parent];
                        heap[parent] = temp;
                        current_index = parent;
                        parent = (current_index - 1) / 2;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        //初始化二叉堆
        public void init()
        {

            for (int i = heap.Length / 2; i > 0; i--)
            {
                Min_heap(i);
            }
        }
        //调整最小二叉堆,调整的是第i棵树
        public void Min_heap(int i)
        {
            if (i < heap_size)
            {
                int left = getLeft(i);
                int right = getRight(i);
                int MinFIndex = i;
                if (left < heap_size && heap[left].F < heap[MinFIndex].F)
                {
                    MinFIndex = left;
                }
                if (right < heap_size && heap[right].F < heap[MinFIndex].F)
                {
                    MinFIndex = right;
                }

                if (MinFIndex != i)
                {
                    //交换父子节点
                    Node temp = heap[MinFIndex];
                    heap[MinFIndex] = heap[i];
                    heap[i] = temp;
                    Min_heap(MinFIndex);
                }
            }

        }
    }
}
