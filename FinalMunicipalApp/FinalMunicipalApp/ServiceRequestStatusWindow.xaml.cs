using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FinalMunicipalApp.DataStructures;
using FinalMunicipalApp.Models;

namespace MunicipalApp.Windows
{
    public partial class ServiceRequestStatusWindow : Window
    {
        private BinarySearchTree bst;
        private AVLTree avl;
        private MinHeap minHeap;
        private Graph graph;
        private List<ServiceRequest> masterList;

        public ServiceRequestStatusWindow()
        {
            InitializeComponent();
            InitializeStructures();
            LoadSampleData(); // replace or adapt to feed from your existing issues list
            RefreshList();
        }

        private void InitializeStructures()
        {
            bst = new BinarySearchTree();
            avl = new AVLTree();
            minHeap = new MinHeap();
            graph = new Graph();
            masterList = new List<ServiceRequest>();
        }

        private void LoadSampleData()
        {
            // If you have existing project data, pull that here instead.
            var samples = new[]
            {
                new ServiceRequest("REQ-001","Pothole on Main St","Large pothole near 12 Main",2, RequestStatus.Submitted),
                new ServiceRequest("REQ-003","Streetlight out","Light not working",3, RequestStatus.InProgress),
                new ServiceRequest("REQ-002","Water leak","Water leaking near 5 Park Ave",1, RequestStatus.Submitted),
                new ServiceRequest("REQ-004","Graffiti","Graffiti on school wall",4, RequestStatus.Completed),
            };

            foreach (var r in samples)
            {
                masterList.Add(r);
                bst.Insert(r);
                avl.Insert(r);
                minHeap.Insert(r);
            }

            // Example: add edges between teams or requests (simulate routing/dependency)
            graph.AddEdge("Depot", "TeamA", 3);
            graph.AddEdge("Depot", "TeamB", 4);
            graph.AddEdge("TeamA", "TeamB", 2);
            graph.AddEdge("TeamB", "TeamC", 5);
        }

        private void RefreshList()
        {
            lstRequests.ItemsSource = null;
            lstRequests.ItemsSource = masterList.OrderBy(r => r.RequestId).ToList();
            statusText.Text = $"Loaded {masterList.Count} requests";
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string id = txtSearchId.Text?.Trim();
            if (string.IsNullOrEmpty(id)) { statusText.Text = "Enter a Request ID"; return; }
            var found = avl.Find(id) ?? bst.Find(id);
            if (found != null)
            {
                txtDetails.Text = found.ToString() + "\n\n" + found.Description;
                statusText.Text = $"Found {found.RequestId}";
                lstRequests.SelectedItem = found;
            }
            else
            {
                txtDetails.Text = "";
                statusText.Text = $"Request {id} not found";
            }
        }

        private void BtnShowNextPriority_Click(object sender, RoutedEventArgs e)
        {
            var next = minHeap.ExtractMin();
            if (next == null) { statusText.Text = "No prioritized requests"; return; }
            txtOutput.Text = $"Next highest priority:\n{next}\n";
            statusText.Text = $"Suggested: {next.RequestId}";
            // Note: we removed from heap but not from master list or trees. If you want to mark dispatched:
            next.Status = RequestStatus.InProgress;
            RefreshList();
        }

        private void BtnInOrder_Click(object sender, RoutedEventArgs e)
        {
            var inOrder = bst.InOrder();
            txtOutput.Text = "In-order by RequestId:\n" + string.Join(Environment.NewLine, inOrder.Select(x => x.ToString()));
            statusText.Text = "Displayed in-order traversal";
        }

        private void BtnBFS_Click(object sender, RoutedEventArgs e)
        {
            var order = graph.BFS("Depot");
            txtOutput.Text = "Graph BFS from Depot:\n" + string.Join(" -> ", order);
            statusText.Text = "Graph BFS completed";
        }

        private void LstRequests_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstRequests.SelectedItem is ServiceRequest r)
            {
                txtDetails.Text = r.ToString() + "\n\n" + r.Description;
            }
        }

        private void BtnUpdateStatus_Click(object sender, RoutedEventArgs e)
        {
            if (lstRequests.SelectedItem is ServiceRequest r)
            {
                var sel = (cmbStatus.SelectedItem as ComboBoxItem)?.Content?.ToString();
                if (Enum.TryParse(sel, out RequestStatus ns))
                {
                    r.Status = ns;
                    txtDetails.Text = r.ToString() + "\n\n" + r.Description;
                    statusText.Text = $"Updated {r.RequestId} to {ns}";
                    RefreshList();
                }
            }
        }
    }
}

