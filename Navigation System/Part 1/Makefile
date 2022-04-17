CC = g++
C_flag = -std=c++11 -Wall
TRGT = server
OBJS = server.o dijkstra.o digraph.o

$(TRGT): $(OBJS)
	$(CC) -o $(TRGT) $(OBJS)

server.o: server.cpp
	$(CC) $(C_flag) -c server.cpp

dijkstra.o: dijkstra.cpp dijkstra.h
	$(CC) $(C_flag) -c dijkstra.cpp

digraph.o: digraph.cpp digraph.h wdigraph.h
	$(CC) $(C_flag) -c digraph.cpp

clean:
	rm -f $(OBJS) $(TRGT)
