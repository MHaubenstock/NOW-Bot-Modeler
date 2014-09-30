function [legflag, RSP, RSR, RER, REY, RWY, LSP, LSR, LER, LEY, LWY]=gradientdescent(framevector, iRSP, iRSR, iRER, iREY, iRWY, iLSP, iLSR, iLER, iLEY, iLWY)
%Gradient Descent Minimization Algorithm
%In this method we will use the gradient of the function at a certain point
%to determine the direction to move
%We will terminate when we reach the goal or close enough to the goal
%We will basically evaluate if cost increased or decreased with the
%direction chosen. The cost will be the distance between the goal and the
%kinematics using the sampled variables
%The leg flag will indicate 1: strafe left, 2: strafe right, 3: Walk
%Forward, 4: Walk Backwards
%The order of the angles are Right side: Shoulder pitch, Shoulder roll,
%Elbow roll, Elbow yaw, Wrist yaw, followed by the left side in the same
%order
%comment the next two lines and uncomment the following
%rand('state',0)
%A=importdata('C:/Users/khalid1/Documents/MATLAB/Debug/SkeletonData0_000.dat');
A=framevector;
%order is shoulder pitch, shoulder roll, elbow roll, elbow yaw, wrist yaw
SCALERATIO=55/250;
goalLEFT=1000*[A(4)-A(10) A(5)-A(11) A(6)-A(12)]*SCALERATIO;
goalRIGHT=1000*[A(22)-A(28) A(23)-A(29) (A(24)-A(30))]*SCALERATIO;
legleft=1000*[A(13)-A(19) A(14)-A(20) A(15)-A(21)];
legright=1000*[A(31)-A(37) A(32)-A(38) A(33)-A(39)];
%Uses leg displacement to determine the actual decision to walk
if legleft(1)<-150
    display('Strafe left');
    legflag=1;
elseif legright(1)>150
    display('Strafe right');
    legflag=2;
elseif legleft(3)>200 || legright(3)>200
    display('Walk Forward')
    legflag=3;
elseif legleft(3)<-150 || legright(3)<-150
    display('Walk Backwards');
    legflag=4;
end
NUMANGLES=5;
MAX=[2.0857 1.3265 -0.0349 2.0857 1.8238];
MIN=[-2.0857 -0.3124 -1.5446 -2.0857 -1.8238];
STEPSIZE=pi/180;
MAXCOST=20;
direction=ones(NUMANGLES, NUMANGLES*2);
costd=ones(NUMANGLES*2,1);
%initialize the array
angles=ones(NUMANGLES,1);
%comment the for loop and uncomment the next 5 lines
%for i=1:NUMANGLES
%    angles(i)=rand*MAX(i)+MIN(i);
%end
angles(1)=iLSP
angles(2)=iLSR
angles(3)=iLER
angles(4)=iLEY
angles(5)=iLWY
%start with arbitrary cost
cost=100;
%main loop
while cost>MAXCOST
%now have the cost function
[q,r,s]=ForwardKinematicsHand(angles(1), angles(2), angles(3), angles(4), angles(5));
cost=sqrt((goalLEFT(1)-q)*(goalLEFT(1)-q)+(goalLEFT(2)-r)*(goalLEFT(2)-r)+((goalLEFT(3)-s)*(goalLEFT(3)-s)));
%now we have to decide which direction to move

%this direction is 2*number of variables
for i=1:NUMANGLES*2
    direction(:,i)=angles(:);
end
for i=1:NUMANGLES
    %find the cost moving up
    direction(i,i)=angles(i)+STEPSIZE;
    direction(i,i+NUMANGLES)=angles(i)-STEPSIZE;
    if(direction(i,i)>MAX(i)) 
        direction(i,i)=MAX(i);
    end
    if(direction(i, i+NUMANGLES)<MIN(i)) 
        direction(i, i+NUMANGLES)=MIN(i);
    end
end
for i=1:NUMANGLES*2
[q,r,s]=ForwardKinematicsHand(direction(1,i), direction(2,i), direction(3,i), direction(4,i), direction(5,i));
costd(i)=sqrt((goalLEFT(1)-q)*(goalLEFT(1)-q)+(goalLEFT(2)-r)*(goalLEFT(2)-r)+((goalLEFT(3)-s)*(goalLEFT(3)-s)));
end
[~, index]=min(costd);
angles=direction(:,index);
end
LSP=angles(1);
LSR=angles(2);
LER=angles(3);
LEY=angles(4);
LWY=angles(5);
display(angles)
display([q r s]);

%This performs the operations on the right hand
direction=ones(NUMANGLES, NUMANGLES*2);
%initialize the array
%comment the for loop and uncomment the next 5 lines
%for i=1:NUMANGLES
%    angles(i)=rand*MAX(i)+MIN(i);
%end
angles(1)=iRSP
angles(2)=iRSR
angles(3)=iRER
angles(4)=iREY
angles(5)=iRWY
%start with arbitrary cost
cost=100;
%main loop
while cost>MAXCOST
%now have the cost function
[q,r,s]=ForwardKinematicsHandOther(angles(1), angles(2), angles(3), angles(4), angles(5));
cost=sqrt((goalRIGHT(1)-q)*(goalRIGHT(1)-q)+(goalRIGHT(2)-r)*(goalRIGHT(2)-r)+((goalRIGHT(3)-s)*(goalRIGHT(3)-s)));
%now we have to decide which direction to move
%this direction is 2*number of variables
for i=1:NUMANGLES*2
    direction(:,i)=angles(:);
end
for i=1:NUMANGLES
    %find the cost moving up
    direction(i,i)=angles(i)+STEPSIZE;
    direction(i,i+NUMANGLES)=angles(i)-STEPSIZE;
    if(direction(i,i)>MAX(i)) 
        direction(i,i)=MAX(i);
    end
    if(direction(i, i+NUMANGLES)<MIN(i)) 
        direction(i, i+NUMANGLES)=MIN(i);
    end
end
for i=1:NUMANGLES*2
[q,r,s]=ForwardKinematicsHandOther(direction(1,i), direction(2,i), direction(3,i), direction(4,i), direction(5,i));
costd(i)=sqrt((goalRIGHT(1)-q)*(goalRIGHT(1)-q)+(goalRIGHT(2)-r)*(goalRIGHT(2)-r)+((goalRIGHT(3)-s)*(goalRIGHT(3)-s)));
end
[~, index]=min(costd);
angles=direction(:,index);
end
RSP=angles(1);
RSR=angles(2);
RER=angles(3);
REY=angles(4);
RWY=angles(5);
display(angles)
display([q r s]);
end