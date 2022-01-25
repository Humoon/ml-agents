from mlagents_envs.environment import UnityEnvironment
from mlagents_envs.side_channel import IncomingMessage, OutgoingMessage
from mlagents_envs.side_channel.engine_configuration_channel import \
    EngineConfigurationChannel
from mlagents_envs.side_channel.stats_side_channel import StatsSideChannel

# Create the channel
stats_channel = StatsSideChannel()
engine_channel = EngineConfigurationChannel()
engine_channel.set_configuration_parameters(time_scale=8)
# We start the communication with the Unity Editor and pass the string_log side channel as input
env = UnityEnvironment(side_channels=[engine_channel, stats_channel])
env.reset()

# string_log.send_string("The environment was reset")

group_name = list(env.behavior_specs.keys())[0]  # Get the first group_name
group_spec = env.behavior_specs[group_name]


def make_msg(key, value):
    msg = OutgoingMessage()
    msg.write_string(key)
    msg.write_int32(value)
    return IncomingMessage(msg.buffer)


# assert len(stats) == 1
# val, method = stats["stats-1"][0]

for i in range(1000):
    decision_steps, terminal_steps = env.get_steps(group_name)
    # We send data to Unity : A string with the number of Agent at each
    # stats_channel.on_message_received(make_msg(key="stats_" + str(i), value=i))

    stats = stats_channel.get_and_reset_stats()

    print(stats)

    env.step()  # Move the simulation forward

env.close()
