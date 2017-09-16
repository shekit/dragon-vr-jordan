#include <Process.h>    // include the Process class of the Bridge lib


// URLS FOR PARAMETERS

String ipAdd = "http://10.0.32.2:8080/";

String upUrl = ipAdd+"up/"; 

String downUrl = ipAdd+"down/"; 

String leftUrl = ipAdd+"left/";

String rightUrl = ipAdd+"right/"; 

String evenUpUrl = ipAdd+"even-up/";

String evenDownUrl = ipAdd+"even-down/";

String evenLeftUrl = ipAdd+"even-left/";

String evenRightUrl = ipAdd+"even-right/";

String rollUrl = ipAdd+"roll/";

// PROCESS VARIABLES
Process up;
Process down;
Process left;
Process right;
Process evenUp;
Process evenDown;
Process evenLeft;
Process evenRight;
Process roll;

// BUTTONS
int rollButton = 12;
int rollButtonState = 0;
int prevRollButtonState = 0;

int upButton = 11;
int upButtonState = 0;
int prevUpButtonState = 0;

int downButton = 10;
int downButtonState = 0;
int prevDownButtonState = 0;

int leftButton = 9;
int leftButtonState = 0;
int prevLeftButtonState = 0;

int rightButton = 8;
int rightButtonState = 0;
int prevRightButtonState = 0;

int fanButton = 4;

bool fanIsOn = false;

void setup() {
  Bridge.begin();      // Initialize Bridge
  Serial.begin(9600);  // Initialize Serial
  
  pinMode(upButton, INPUT);
  pinMode(downButton, INPUT);
  pinMode(leftButton, INPUT);
  pinMode(rightButton, INPUT);
  pinMode(fanButton, OUTPUT);
  pinMode(13, OUTPUT);
}

void loop() {
  
  upButtonState = digitalRead(upButton);
  downButtonState = digitalRead(downButton);
  leftButtonState = digitalRead(leftButton);
  rightButtonState = digitalRead(rightButton);
  rollButtonState = digitalRead(rollButton);
  
  if(rollButtonState == HIGH && rollButtonState != prevRollButtonState){
     roll.begin("curl");
     roll.addParameter(rollUrl);
     roll.run();  
  }
  prevRollButtonState = rollButtonState;
  
  if(upButtonState == HIGH){
    //Serial.println("up");
    prevUpButtonState = HIGH;
    
    up.begin("curl");
    up.addParameter(upUrl);
    up.run();
    digitalWrite(13, HIGH);
  }
  
  if(upButtonState == LOW && prevUpButtonState == HIGH){
     //Serial.println("up button released");
     prevUpButtonState = LOW;
     evenUp.begin("curl");
     evenUp.addParameter(evenUpUrl);
     evenUp.run(); 
  }
  
  if(downButtonState == HIGH){
     //Serial.println("down");
     fanIsOn = true;
     prevDownButtonState = HIGH;
     down.begin("curl");
     down.addParameter(downUrl);
     down.run();
  }
  
  if(downButtonState == LOW && prevDownButtonState == HIGH){
     //Serial.println("down button released");
     fanIsOn = false;
     prevDownButtonState = LOW;
     
     evenDown.begin("curl");
     evenDown.addParameter(evenDownUrl);
     evenDown.run(); 
  }
  
  if(leftButtonState == HIGH){
     //Serial.println("left");
     prevLeftButtonState = HIGH;
     left.begin("curl");
     left.addParameter(leftUrl);
     left.run(); 
  }
  
  if(leftButtonState == LOW && prevLeftButtonState == HIGH){
    //Serial.println("left button released");
     prevLeftButtonState = LOW;
     evenLeft.begin("curl");
     evenLeft.addParameter(evenLeftUrl);
     evenLeft.run(); 
  }
  
  if(rightButtonState == HIGH){
    //Serial.println("right");
     prevRightButtonState = HIGH;
     right.begin("curl");
     right.addParameter(rightUrl);
     right.run(); 
  }
  
  if(rightButtonState == LOW && prevRightButtonState == HIGH){
    //Serial.println("right button released");
     prevRightButtonState = LOW;
     evenRight.begin("curl");
     evenRight.addParameter(evenRightUrl);
     evenRight.run(); 
  }
  
  if(fanIsOn){
    digitalWrite(fanButton, HIGH);
  } else {
    digitalWrite(fanButton, LOW);
  }

 
  delay(20);
 

}

