Municipal Services Application — Part 3 (Service Request Status)

---

## Overview
This project is the final phase (Part 3) of the **Municipal Services Application** for South Africa.  
It expands upon **Part 1 (Report Issues)** and **Part 2 (Local Events & Announcements)** by introducing the **Service Request Status** functionality.

The application allows residents to:
- Report municipal service issues.
- View local community events and announcements.
- Track the progress and status of submitted service requests.

---

## Technologies Used
- **C# (WPF)**
- **.NET Framework / .NET 6 (depending on setup)**
- **Visual Studio 2022**
- **Object-Oriented Programming**
- **Custom Data Structures (Trees, Graphs, Heaps)**

---

## Application Features

### Main Menu
A Windows Form (MainWindow) that provides navigation options:
- **Report Issues** (from Part 1)
- **Local Events & Announcements** (from Part 2)
- **Service Request Status** (Part 3 – New)

### Service Request Status Window
The *ServiceRequestStatusWindow* displays and manages all submitted requests:
- View all service requests and their statuses.
- Search and track requests by **unique ID**.
- Requests stored and retrieved using advanced data structures for efficiency.

---

## Data Structures Implemented

| Data Structure | Purpose | Description |
|----------------|----------|-------------|
| **Binary Search Tree (BST)** | Organizing Service Requests | Enables fast searching, insertion, and retrieval by Request ID. |
| **AVL Tree** | Balanced Data Retrieval | Maintains sorted requests with automatic balancing for efficiency. |
| **Heap** | Priority Management | Manages urgent requests by priority (e.g., "Water Leak" before "Noise Complaint"). |
| **Graph** | Relationship Representation | Models connections between municipal departments handling different types of service requests. |
| **Graph Traversal (DFS/BFS)** | Department Search | Used to find the shortest path to the responsible department. |
| **Minimum Spanning Tree (MST)** | Optimization | Simulates optimizing resource allocation between departments for efficiency. |

---

## How to Compile and Run the Application

### Requirements
- **Visual Studio 2022 (or later)**
- **.NET Framework 4.7.2+** or **.NET 6 WPF Desktop**

### Steps to Run
1. **Clone the repository:**
   ```bash
   git clone 
