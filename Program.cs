using System;
using System.Collections.Generic;
using System.Linq;


namespace Rocket_Elevators_Corporate_Controller
{
    class Program
    {
        // Sanchez is the name of my Controller
        // all begin here that the initial battery

        static void Main(string[] args)
        {
            //Battery properties (Building Floor), (Number of Column), (Number of Elevators per Column)
            ElevatorController Sanchez1 = new ElevatorController(85, 4, 5);// battery properties
            Console.WriteLine("Starting initial Battery");
            Console.WriteLine("Column number is : " + Sanchez1.nbColumns);
            Console.WriteLine("Number of total floors is = " + Sanchez1.BuildingFloorNumber);
            Console.WriteLine("Battery name is :  " + Sanchez1);


            Sanchez1.Battery1.columnList[1].elevatorList[0].FloorNumber = 1;
            Sanchez1.Battery1.columnList[1].elevatorList[0].AddFloorToList(24);
            Sanchez1.Battery1.columnList[1].elevatorList[1].FloorNumber = 21;
            Sanchez1.Battery1.columnList[1].elevatorList[1].AddFloorToList(28);
            Sanchez1.Battery1.columnList[1].elevatorList[2].FloorNumber = 33;
            Sanchez1.Battery1.columnList[1].elevatorList[2].AddFloorToList(1);
            Sanchez1.Battery1.columnList[1].elevatorList[3].FloorNumber = 40;
            Sanchez1.Battery1.columnList[1].elevatorList[3].AddFloorToList(1);
            Sanchez1.Battery1.columnList[1].elevatorList[4].FloorNumber = 42;


            Sanchez1.AssignElevator(36);
            Console.ReadLine();
        }
    }


    class ElevatorController
    {

        //This is the elevator with is function
        public int BuildingFloorNumber;
        public int nbColumns;
        public int nbElevatorsByColumns;
        public Battery Battery1;
        public ElevatorController(int BuildingFloorNumber, int nbColumns, int nbElevatorsByColumns)
        {
            this.nbElevatorsByColumns = nbElevatorsByColumns;
            this.BuildingFloorNumber = BuildingFloorNumber;
            this.nbColumns = nbColumns;
            this.Battery1 = new Battery(BuildingFloorNumber, nbColumns, nbElevatorsByColumns);
        }


        //elevator taking request of people with
        //requested floor number and number of floors
        //and send the nearest elevator after it s send
        // to a floor list with requested floor number
        //they send the nearest elevator to people
        public void RequestElevator(int FloorNumber, int requestedFloorNumber)
        {

            var column1 = Battery1.FindColumn(requestedFloorNumber);
            var nearestElevator = column1.FindNearestElevator(1);
            column1.CreateElevators();


            Console.WriteLine("someone requested an elevator at floor: " + requestedFloorNumber);        
            Console.WriteLine("returning elevator: "+ nearestElevator.elevatorName + " of column number "+ column1.columnName);

                nearestElevator.AddFloorToList(requestedFloorNumber);
                nearestElevator.MoveNext();

           

        }


        // its gonna  assign the best elevator depending where the user is going to
        public void AssignElevator(int requestedFloorNumber)
        {

            Battery1.DisplayMainPanel(requestedFloorNumber);
            var column1 = Battery1.FindColumn(requestedFloorNumber);
            var nearestElevator = column1.FindNearestElevator(requestedFloorNumber);


            Console.WriteLine("Requested Floor at : " + requestedFloorNumber);
            Console.WriteLine("Returning Elevator: " + nearestElevator.elevatorName + " of Column number " + column1.columnName);

            nearestElevator.AddFloorToList(requestedFloorNumber);
            nearestElevator.MoveNext();

            nearestElevator.CallButtonLightOn();

        }
    }


    class Battery
    {
        //battery operate column and screen display panel
        // for people asking for elevator 

        public int nbFloors;
        public int nbColumns;
        public int nbElevatorsByColumns;
        public List<Column> columnList;
        public Battery(int nbFloors, int nbColumns, int nbElevatorsByColumns)
        {

            this.nbFloors = nbFloors;
            this.nbColumns = nbColumns;
            this.nbElevatorsByColumns = nbElevatorsByColumns;
            this.columnList = new List<Column>();
            this.CreateColumns();

        }



        //  where columns is creating      
        public void CreateColumns()
        {

            for (int i = 0; i < this.nbColumns; i++)
            {
                var columns = new Column(nbFloors, nbElevatorsByColumns, "Column " + (i +1));
                columnList.Add(columns);
            }
        }


        //   Screen display panel where people choice for his level to go
        //   and show the number in screen of people choice
        public void DisplayMainPanel(int requestedFloorNumber)
        {

            Console.WriteLine("someone enter number on display panel: " + requestedFloorNumber);

        }

        // each columns have a range to deserved
        // and it s associate to people request
        public Column FindColumn(int requestedFloor)
        {

            if (requestedFloor <= 22)
            {
                return columnList[0];
            }
            else if (requestedFloor == 1 ||(requestedFloor >= 23 && requestedFloor <= 43))
            {
                return columnList[1];
            }
            else if (requestedFloor == 1 || (requestedFloor >= 44 && requestedFloor <= 64))
            {
                return columnList[2];
            }
            else
            {
                return columnList[3];
            }
        }
    }


    class Column
    {
        // the column section control elevator and,
        //  his feature like elevator list and call button list

        public int nbFloors;
        public int nbElevators;
        public string columnName;
        public List<Elevator> elevatorList;
        public List<CallButton> callButtonList;
        public Column(int nbFloors, int nbElevators, string columnName)
        {
            this.nbFloors = nbFloors;
            this.nbElevators = nbElevators;
            this.columnName = columnName;
            this.callButtonList = new List<CallButton>();
            this.elevatorList = new List<Elevator>();
            this.CreateElevators();
            this.CreateCallButtons();
        }


        // creating elevator and push it in elevator list
        public void CreateElevators()
        {
        
            for (int i = 0; i < this.nbElevators; i++)
            {
                var elevators = new Elevator(i + 1, 1);
                this.elevatorList.Add(elevators);
            }
        }


        // the call button list is create with the number of floors
        // after they make a list and push it inside 
        public void CreateCallButtons()
        {

            for (var i = 0; i < nbFloors; i++)
            {

                var callbutton = new CallButton(i, "Down");
                if (i != 0)
                {

                    this.callButtonList.Add(callbutton);
                }
            }
        }


        // find the best elevator with requested floor number and number of  floors
        // making the difference for send the best elevator for people request
        //making a for loop with elevator list
        public Elevator FindNearestElevator(int requestedFloorNumber)
        {
            var differenceFloor = 27;
            var bestDifference = 100;
            var nearestElevator = 1;


            for (var i = 0; i < elevatorList.Count; i++) { 
            
                 differenceFloor = Math.Abs(requestedFloorNumber - elevatorList[i].FloorNumber);

                if (differenceFloor < bestDifference)
                {
                    nearestElevator = i;
                    bestDifference = differenceFloor;
                }
            }
            return elevatorList[nearestElevator];
        }
    }


    // button using the requested floor and direction of people requested
    class CallButton
    {

        public int requestedFloor;
        public string direction;
        public bool activateButton;
        public CallButton(int requestedFloor, string direction)
        {

            this.requestedFloor = requestedFloor;
            this.direction = "Down";
            this.activateButton = false;
        }
    }


    class Elevator
    {

        //class elevator using number of floor and direction
        // using open and closing doors
        // control the display panel where people choice is floor to go
        public int elevatorName;
        public string direction;
        public string status;
        public int FloorNumber;
        public List<int> requestFloorList;
        public Elevator(int elevatorName, int FloorNumber) { 
        
            this.elevatorName = elevatorName;
            this.FloorNumber = FloorNumber;
            this.direction = "Stopped";
            this.status = "Idle";
            requestFloorList = new List<int>();
        }


        public void MoveNext()
        {

            int requestedFloorNumber = requestFloorList[0];
            while (requestedFloorNumber != 1) { 
            
                if (FloorNumber == requestedFloorNumber)
                {

                    // make move elevator with interval of 0.1 sec by floor level
                    // with number of requested floor number an Floor Number
                    CallButtonLightOff();
                    OpeningAndClosingDoors();
                    requestedFloorNumber = ResetRequestedFloorNumber();
              
                }

                else if (requestedFloorNumber > FloorNumber) { 
                
                    MoveUp();
                    System.Threading.Thread.Sleep(100);
                    Console.WriteLine(FloorNumber);
                }

                else { 
                
                    MoveDown();
                    System.Threading.Thread.Sleep(100);
                    Console.WriteLine(FloorNumber);
                }
            }
            CallButtonLightOn();
            ResetElevatorToFirstLobby();
            CallButtonLightOff();
            OpeningAndClosingDoors();
        }


        //making a timer in  for open door
        // making a timer  for closing door 
        public void OpeningAndClosingDoors() {

          
            Console.WriteLine("Open doors");
            System.Threading.Thread.Sleep(100);
            Console.WriteLine("Close Doors");
            System.Threading.Thread.Sleep(100);
        }


        // this is light "ON" for call button 
        public void CallButtonLightOn() { 
        
            Console.WriteLine("Call Button Light ON");
        }


        // this is light "OFF" for call button
        public void CallButtonLightOff() { 
        
            Console.WriteLine("Call Button Light OFF");
        }


        //When the elevator is done is job with the task in the list
        //its gonna remove the floor from the request floor list 
        private int ResetRequestedFloorNumber() { 
        

            requestFloorList.RemoveAt(0);
            int requestedFloorNumber = 1;
            if (requestFloorList.Count != 0)
            {
                requestedFloorNumber = requestFloorList[0];
            }

            return requestedFloorNumber;
        }

        //this is method for elevator return to lobby
        //with timer set
        public void ResetElevatorToFirstLobby() { 
        

            while (FloorNumber > 1) { 
            

                MoveDown();
                System.Threading.Thread.Sleep(10);
                Console.WriteLine(FloorNumber);
            }
        }


        // move up using Floor Number +1
        public void MoveUp() { 
        
            FloorNumber++;
            Console.WriteLine("Elevator is moving up");
        }


        // moving down using Floor Number -1
        public void MoveDown() { 
        
            FloorNumber--;
            Console.WriteLine("Elevator is moving Down");
        }


        // Add the requested of people and add it in request floor list
        // the requested floor is making by descending  
        public void AddFloorToList(int requestedFloorNumber) { 
        

            requestFloorList.Add(requestedFloorNumber);
            requestFloorList = requestFloorList.OrderByDescending(x => x).ToList();
        }
    }
}
