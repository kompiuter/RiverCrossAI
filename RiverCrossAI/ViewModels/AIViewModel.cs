using RiverCrossAI.Codes;
using RiverCrossAI.Common;
using RiverCrossAI.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RiverCrossAI.ViewModels
{
    class AIViewModel : BindableBase
    {
        #region Properties

        private int _expandedCount = 0;
        /// <summary>
        /// Indicates how many states have been expanded
        /// </summary>
        public int ExpandedCount
        {
            get { return _expandedCount; }
            set { Set(ref _expandedCount, value); }
        }

        private double _delaySpeed;
        /// <summary>
        /// Speed at which AI delays 'thinking'. Increase delay to make results
        /// on UI clearer
        /// </summary>
        public double DelaySpeed
        {
            get { return _delaySpeed; }
            set
            {
                if (value < 0)
                    value = 0;

                Set(ref _delaySpeed, value);
            }
        }

        private bool _goalReached = false;
        /// <summary>
        /// Indicates whether search has successfully reached the goal state
        /// </summary>
        public bool GoalReached
        {
            get { return _goalReached; }
            set { Set(ref _goalReached, value); }
        }

        private bool _isBusy = false;
        /// <summary>
        /// Indicates whether search has successfully reached the goal state
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            set { Set(ref _isBusy, value); }
        }

        private bool _bfsEnabled = true;
        /// <summary>
        /// Indicates whether search should be done with BFS or DFS
        /// </summary>
        public bool BFSEnabled
        {
            get { return _bfsEnabled; }
            set { Set(ref _bfsEnabled, value); }
        }

        private ObservableCollection<Vector3> _openStates = new ObservableCollection<Vector3>();
        public ObservableCollection<Vector3> OpenStates
        {
            get { return _openStates; }
            set { Set(ref _openStates, value); }
        }

        private ObservableCollection<Vector3> _closedStates = new ObservableCollection<Vector3>();
        public ObservableCollection<Vector3> ClosedStates
        {
            get { return _closedStates; }
            set { Set(ref _closedStates, value); }
        }


        #endregion

        #region Fields

        /// <summary>
        /// The initial state of the problem, where there are 3 missionaries
        /// & 3 cannibals on the start side of the river and the boat is present
        /// </summary>
        protected Vector3 InitialState => new Vector3(3, 3, 1);

        /// <summary>
        /// The goal state of the problem, where there are no missionaries 
        /// & no cannibals on the start side of the river and the boat is not present
        /// </summary>
        protected Vector3 GoalState => new Vector3(0, 0, 0);

        /// <summary>
        /// Operator for moving 1 missionary with the boat
        /// </summary>
        Func<Vector3, Vector3> Move1MFunc = (v) => v.Z == 0 ? v.Add(1, 0, 1) : v.Subtract(1, 0, 1);

        /// <summary>
        /// Operator for moving 2 missionaries with the boat
        /// </summary>
        Func<Vector3, Vector3> Move2MFunc = (v) => v.Z == 0 ? v.Add(2, 0, 1) : v.Subtract(2, 0, 1);

        /// <summary>
        /// Operator for moving 1 cannibal with the boat
        /// </summary>
        Func<Vector3, Vector3> Move1CFunc = (v) => v.Z == 0 ? v.Add(0, 1, 1) : v.Subtract(0, 1, 1);

        /// <summary>
        /// Operator for moving 2 cannibals with the boat
        /// </summary>
        Func<Vector3, Vector3> Move2CFunc = (v) => v.Z == 0 ? v.Add(0, 2, 1) : v.Subtract(0, 2, 1);

        /// <summary>
        /// Operator for moving 1 missionary & 1 cannibal with the boat
        /// </summary>
        Func<Vector3, Vector3> MoveMCFunc = (v) => v.Z == 0 ? v.Add(1, 1, 1) : v.Subtract(1, 1, 1);

        protected List<Func<Vector3, Vector3>> Operators { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Starts searching the tree for the goal solution
        /// </summary>
        protected async Task ExecuteBeginSearch()
        {
            try
            {
                InitialiseValues();

                IsBusy = true;

                // Add initial state to OpenStates queue
                OpenStates.Insert(0, InitialState);

                // Loop until currentstate is not equal to goal state 
                // or until openstates queue is empty
                while (OpenStates.Count > 0)
                {
                    var currentState = OpenStates.First();

                    ExpandedCount++;

                    // If goal state is reached, exit loop
                    if (currentState.Equals(GoalState))
                    {
                        GoalReached = true;
                        break;
                    }

                    OpenStates.Remove(currentState);

                    // Add current state to the queue
                    ClosedStates.Add(currentState);

                    // Used to index entries in a DFS search to store them as a stack
                    var DFScount = 0;

                    foreach (var childState in GetChildrenStates(currentState))
                    {
                        // If child state is not contained in the open or closed queue, then enqueue it,
                        // else discard it as it has already been expanded
                        if (!OpenStates.Any(s => s.Equals(childState)) && !ClosedStates.Any(s => s.Equals(childState)))
                        {
                            // If BFS is enabled, then add the item to the end of the collection so it acts as a queue
                            if (BFSEnabled)
                                OpenStates.Insert(OpenStates.Count, childState);
                            // If DFS is enabled, add items to the beginning of the collection so it acts as a stack
                            else
                                OpenStates.Insert(DFScount++, childState);

                            // Delay here if user has chosen so
                            await Task.Delay(Convert.ToInt32(DelaySpeed));
                        }
                    }
                }
            }
            finally { IsBusy = false; }          
        }

        private void InitialiseValues()
        {
            // Initialise list of operators
            if (Operators == null)
            {
                Operators = new List<Func<Vector3, Vector3>>();
                Operators.Add(Move1MFunc);
                Operators.Add(Move2MFunc);
                Operators.Add(Move1CFunc);
                Operators.Add(Move2CFunc);
                Operators.Add(MoveMCFunc);
            }

            // Set count to 0
            ExpandedCount = 0;

            // Clear collections
            OpenStates = new ObservableCollection<Vector3>();
            ClosedStates = new ObservableCollection<Vector3>();

            GoalReached = false;
        }

        /// <summary>
        /// Expands a state and returns all its children
        /// </summary>
        /// <param name="parentState">Parent state to expand for children</param>
        /// <returns>Ienumerable of children states</returns>
        protected IEnumerable<Vector3> GetChildrenStates(Vector3 parentState)
        {
            foreach (var op in Operators)
            {
                // Get a child using the current operator
                var possibleChild = op(parentState);

                // If state is a valid state, return it
                if (IsValidState(possibleChild))
                    yield return possibleChild;
            }

        }

        /// <summary>
        /// Determines whether a state is valid based on the rules of the problem
        /// </summary>
        /// <param name="state">The state to check whether it is valid or not</param>
        /// <returns>Boolean: true if valid, false otherwise</returns>
        protected bool IsValidState(Vector3 state)
        {
            // 'Impossible' scenarios removed
            if (state.X > 3 || state.Y > 3 || state.X < 0 || state.Y < 0)
                return false;

            // If there are 3 missionaries on either side, then it's a valid state
            // no matter where the boat are cannibals are
            if (state.X == 3 || state.X == 0)
                return true;

            // If there are the same number of cannibals and missionaries on either side,
            // then it is a valid state regardless of the boat
            if (state.X == state.Y)
                return true;

            // All other states are invalid
            return false;
        }

        #endregion

        #region Commands

        private RelayCommand _beginSearchCommand;
        public RelayCommand BeginSearchCommand
        {
            get
            {
                if (_beginSearchCommand == null)
                    _beginSearchCommand = new RelayCommand(async (p) => await ExecuteBeginSearch(), (p) => !IsBusy);

                return _beginSearchCommand;                
            }
        }


        #endregion
    }
}
