//  ---------------------
//  Name: Bashar Harash
//  CCID: HARASH
//  CMPUT 275, WINTER 2022
//  Assignment part I
//  ---------------------

#include <queue>
#include <vector>
#include <functional>
#include <stdio.h>
#include <assert.h>
#include "dijkstra.h"

using namespace std;

// slightly modified version of greater to queue up
// items properly
class GREATER {
public:
    // modified function call operator ()
    bool operator() (const PIPIL& lhs, const PIPIL& rhs) const {
        // return minheap
        return (lhs.second.second > rhs.second.second);
    }
};

/*
    Description: Compute least cost paths that start from a given vertex.
                 Use a binary heap to efficiently retrieve an unexplored
                 vertex that has the minimum distance from the start vertex
                 at every iteration.
                 NOTE: PIL is an alias for "pair<int, long long>" type as discussed in class

    Parameters:
        WDigraph: an instance of the weighted directed graph (WDigraph) class
        startVertex: a vertex in this graph which serves as the root of the search tree
        tree: a search tree to construct the least cost path from startVertex to some vertex

    Return:
        NONE
*/
void dijkstra(const WDigraph& graph, int startVertex, unordered_map<int, PIL>& tree) {
    int endVertex, edgeCost, currVertex;
    int C_flag = 0;  // for debug
    // implementation of the priority queue that act like a min heap
    priority_queue<PIPIL, std::vector<PIPIL>, GREATER > minheap;
    // starVertex corresponds to currVertex = 0, -1 assumes
    // no edgeCost are present
    minheap.push(PIPIL(startVertex, PIL(startVertex, -1)));
    // once the queue is empty, stop
    while (!minheap.empty()) {
        // calculate the target values
        currVertex = minheap.top().first;
        endVertex = minheap.top().second.first;
        edgeCost = minheap.top().second.second;
        // first item in the queue has been processed
        minheap.pop();  // remove the processed item from the queue
        if (tree.find(currVertex) != tree.end()) {
            // move on, endVertex reached
            assert (C_flag == 0);  // for debug
        } else {
            // add endVertex to PIL that hold edgeCost and currVertex
            tree[currVertex] = PIL(endVertex, edgeCost);
            // find all vertices from all edges exiting the currentVertex
            for (auto itr = graph.neighbours(currVertex); itr != graph.endIterator(currVertex); itr++) {
                // new distance = old distance + the cost(i.e. the currVertex) from 
                // new vertex to neighbor vertex
                long long newDistance = edgeCost + graph.getCost(currVertex, *itr);
                // queue next element
                minheap.push(PIPIL(*itr, PIL(currVertex, newDistance)));
            }
        }    
    }
}
