import matplotlib.pyplot as plt
from math import sqrt
from pandas import DataFrame

pos_x_asteroid=[0]
pos_y_asteroid=[0]
vel_x_asteroid=[0]
vel_y_asteroid=[3]

pos_x_earth=5
pos_y_earth=0
mass_earth=50

dt=0.000005

def get_new_velocity():
    distance=sqrt((pos_x_earth - pos_x_asteroid[-1]) ** 2 + (pos_y_earth - pos_y_asteroid[-1]) ** 2)
    G_x= (pos_x_earth - pos_x_asteroid[-1]) * mass_earth / distance ** 3
    G_y= (pos_y_earth - pos_y_asteroid[-1]) * mass_earth / distance ** 3
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

for i in range(5000000):
    new_velocity=get_new_velocity()

    vel_x_asteroid.append(new_velocity[0])
    vel_y_asteroid.append(new_velocity[1])

    new_position=get_new_position()

    pos_x_asteroid.append(new_position[0])
    pos_y_asteroid.append(new_position[1])


plt.plot(pos_x_asteroid,pos_y_asteroid, 'bo')
plt.plot(pos_x_earth,pos_y_earth,'go')
plt.show()

#dict = {'x': pos_x_asteroid, 'y': pos_y_asteroid}

#df = DataFrame(dict)
#df.to_csv("asteroid_trajectory.csv")
#print(df)


