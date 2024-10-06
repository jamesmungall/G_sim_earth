import matplotlib.pyplot as plt
from math import sqrt
from pandas import DataFrame

pos_x_asteroid=[0]
pos_y_asteroid=[0]
vel_x_asteroid=[1]
vel_y_asteroid=[1]

pos_x_earth=5
pos_y_earth=0
mass_earth=500

pos_x_sun=0
pos_y_sun=5
mass_sun=5000

dt=0.0000005

def get_new_velocity(pos_x,pos_y,mass):
    distance=sqrt((pos_x - pos_x_asteroid[-1]) * (pos_x - pos_x_asteroid[-1]) + (pos_y - pos_y_asteroid[-1]) * (pos_y - pos_y_asteroid[-1]))
    G_x= (pos_x - pos_x_asteroid[-1]) * mass / (distance * distance * distance)
    G_y= (pos_y - pos_y_asteroid[-1]) * mass / (distance * distance * distance)
    change_velocity_x=dt*G_x
    change_velocity_y=dt*G_y
    new_velocity_x=vel_x_asteroid[-1]+change_velocity_x
    new_velocity_y=vel_y_asteroid[-1]+change_velocity_y
    return [new_velocity_x, new_velocity_y]

def get_new_position():
    average_vel_x=(vel_x_asteroid[-2]+vel_x_asteroid[-1])/2
    average_vel_y = (vel_y_asteroid[-2] + vel_y_asteroid[-1]) / 2
    change_pos_x=average_vel_x*dt
    change_pos_y=average_vel_y*dt
    new_pos_x=pos_x_asteroid[-1]+change_pos_x
    new_pos_y = pos_y_asteroid[-1] + change_pos_y
    return [new_pos_x, new_pos_y]

for i in range(5000000000000):
    new_velocity_earth=get_new_velocity(pos_x_earth,pos_y_earth,mass_earth)
    new_velocity_sun=get_new_velocity(pos_x_sun,pos_y_sun,mass_sun)

    vel_x_asteroid.append((new_velocity_earth[0]+new_velocity_sun[0])/2)
    vel_y_asteroid.append((new_velocity_earth[1]+new_velocity_sun[1])/2)

    new_position=get_new_position()

    pos_x_asteroid.append(new_position[0])
    pos_y_asteroid.append(new_position[1])


plt.plot(pos_x_asteroid,pos_y_asteroid, 'bo')
plt.plot(pos_x_earth,pos_y_earth,'go')
plt.plot(pos_x_sun,pos_y_sun,'go')
plt.axis((-10,10,-10,10))
plt.show()

#dict = {'x': pos_x_asteroid, 'y': pos_y_asteroid}

#df = DataFrame(dict)
#df.to_csv("asteroid_trajectory.csv")
#print(df)


