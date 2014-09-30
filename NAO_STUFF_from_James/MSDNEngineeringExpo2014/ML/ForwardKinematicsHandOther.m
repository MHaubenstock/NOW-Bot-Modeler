function [x,y,z]=ForwardKinematicsHandOther(ShoulderPitch, ShoulderRoll, ElbowRoll, ElbowYaw, WristYaw)
UpperArmLength=	105.00;
LowerArmLength=	55.95;
HandOffsetX=	57.75;
%DH in order a, alpha, d, theta
DH=[0 pi/2 0 ShoulderPitch; UpperArmLength 0 0 ShoulderRoll; 0 pi/2 0 ElbowRoll; LowerArmLength 0 0 ElbowYaw; HandOffsetX 0 0 WristYaw];
A=ones(4,4,5);
for i=1:5
A(:,:,i)=[cos(DH(i,4)) -sin(DH(i,4))*cos(DH(i,2)) sin(DH(i,4))*sin(DH(i,2)) DH(i,1)*cos(DH(i,4)); sin(DH(i,4)) cos(DH(i,4))*cos(DH(i,2)) -cos(DH(i,4))*sin(DH(i,2)) DH(i,1)*sin(DH(i,4)); 0 sin(DH(i,2)) cos(DH(i,2)) DH(i,3); 0 0 0 1];
end
endeff=[1 0 0 0; 0 1 0 0; 0 0 1 0; 0 0 0 1];
for i=1:5
    endeff=endeff*A(:,:,i);
end
x=endeff(1,4);
y=endeff(2,4);
z=endeff(3,4);
end
    
