//  ---------------------
//  Name: Bashar Harash
//  CCID: HARASH
//  CMPUT 275, WINTER 2022
//  Assignment part I
//  ---------------------

#include <iostream>
#include <string>
#include <fstream>
#include <unordered_map>
#include <list>
#include <sstream>
#include <cmath>
#include "wdigraph.h"
#include "dijkstra.h"
using namespace std;

// structure that stores the latitude and longitude
struct Point {
	long long lat;
	long long lon;
};

/*
	Description: Read Edmonton map data from the provided file and
				 load it into the WDigraph object passed to this function.
				 Store vertex coordinates in Point struct and map each
				 vertex identifier to its corresponding Point struct variable.
	Arguments:
		filename: type string, the name of the file that describes a road network
		graph: type WDigraph, an object which the file get loaded to.
		points: type unordered map, mapping between vertex identifiers and their
			    coordinates.
	Returns:
		None	
*/
void readGraph(string filename, WDigraph& graph, unordered_map<int, Point>& points);

/*
	Description: computes the cost of an edge read from the road network
			 	 file.

	Arguments:
		pt1: type const to referecnce Point
		pt2: type const to referecnce Point

	Returns:
		the manhattan distance type long long
*/
long long manhattan(const Point& pt1, const Point& pt2);

int main() {
	// target file
	string filename = "edmonton-roads-2.0.1.txt";
	// initilize graph
	WDigraph graph;
	unordered_map<int, Point> points;
	// read data from target file
	readGraph(filename, graph, points);
	// latS, lonS, latE and lonE represent latitude and longitude
	// start and end coordinates
	// Xstart and Xend, represents the previous start and end
	long long latS, lonS, latE, lonE, Xstart, Xend;
	int startVertex, endVertex;
	char request;
	cin >> request;
	if (request == 'R') {
		// valid request, ask for input
		cin >> latS >> lonS >> latE >> lonE;
		// map the vertices and the distance between
		unordered_map<int, PIL> tree;
		// search for the shortest path
		for (auto& it : points) {
			if (it.first == points.begin()->first) {
				// save the initial distances (visited only once in the first iteration)
				Xstart = abs(it.second.lat - latS) + abs(it.second.lon - lonS);
				Xend = abs(it.second.lat - latE) + abs(it.second.lon - lonE);
			}
			// new smaller value found
			if (abs(it.second.lat - latS) + abs(it.second.lon - lonS) < Xstart) {
				// update the targets
				Xstart = abs(it.second.lat - latS) + abs(it.second.lon - lonS);
				startVertex = it.first;
			}
			// new smaller value found
			if (abs(it.second.lat - latE) + abs(it.second.lon - lonE) < Xend) {
				// update the targets
				Xend = abs(it.second.lat - latE) + abs(it.second.lon - lonE);
				endVertex = it.first;
			}
		}
		// find the the minimum cost path
		dijkstra(graph, startVertex, tree);
	    if (tree.find(endVertex) != tree.end()) {  // path was not found
	    	list<int> path;
	      	while (endVertex != startVertex) {
	      		// add the endVertex to the list
		        path.push_front(endVertex);
		        // move to the next item
		        endVertex = tree[endVertex].first;
	      	}
	      	path.push_front(startVertex);
	      	// print number of waypoints
	      	cout << "N " << path.size() << endl;
	      	char request = ' ';  // reset the request
	      	while (request != 'A') {
	      		// request to continue
	      		cin >> request;
	      	}
	  		for (auto index : path) {
	  			request = ' ';  // reset the request
	  			// print waypoint
	  			cout << "W " << points[index].lat << " " <<	points[index].lon << endl;
	  			while (request != 'A') {
	  				cin >> request;  // requst 'A'
	  			}
	  		}
	      	cout << "E" << endl;  // End
	    } else {
	    	cout << "N 0" << endl;
	    	cout << "E" << endl;
	    }
	}

	return 0;
}

void readGraph(string filename, WDigraph& graph, unordered_map<int, Point>& points) {
	// initialize line and temp parts of a file
	string line, cache;
	// initilize vertex/edge components
	int ID, startVertex, endVertex;
	// initilize coord type double that get converted later
	// to long long
	double coord;
	// latLL & lonLL hold the converted value of coord
	long long latLL, lonLL, cost;
	// open file
	ifstream target_file(filename);
	// read line by line
	while (getline(target_file, line)) {
		stringstream line_stream(line);  // chunk of a line
		getline(line_stream, cache, ',');
			if (cache == "V") {  // vertex found
				getline(line_stream, cache, ',');
				ID = stoi(cache);  // cast to int
				graph.addVertex(ID);  // add as vertex
				getline(line_stream, cache, ',');
				// store latitude coor in 100,000-ths of a degree
				coord = stod(cache);
				getline(line_stream, cache, ',');
				latLL = static_cast < long long > (coord*100000);
				// store longitude coor in 100,000-ths of a degree
				coord = stod(cache);
				lonLL = static_cast < long long > (coord*100000);
				// store corresponidng coor
				points[ID] = {latLL, lonLL};
			} else if (cache == "E") {  // edge found
				getline(line_stream, cache, ',');
				startVertex = stoi(cache);  // cast to int
				getline(line_stream, cache, ',');
				endVertex = stoi(cache);
				getline(line_stream, cache, ',');
				// calculate the manhattan distance between the coor
				cost = manhattan(points[startVertex], points[endVertex]);
				graph.addEdge(startVertex, endVertex, cost);  // add as edge
			}
	}
	// close file
	target_file.close();
}

long long manhattan(const Point& pt1, const Point& pt2) {
	// calculate the absolute difference of the horizontal distance
	// calculate the absolute difference of the vertical distance
	// return the sum aka manhattan distance
	return abs(pt1.lat - pt2.lat) + abs(pt1.lon - pt2.lon);
}
