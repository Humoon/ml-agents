from mlagents_envs.environment import UnityEnvironment
from mlagents_envs.side_channel.string_log_channel import StringLogChannel

# Create the channel
string_log = StringLogChannel()

# We start the communication with the Unity Editor and pass the string_log side channel as input
env = UnityEnvironment(side_channels=[string_log])
env.reset()
string_log.send_string("The environment was reset")

group_name = list(env.behavior_specs.keys())[0]  # Get the first group_name
group_spec = env.behavior_specs[group_name]
for i in range(100):
    decision_steps, terminal_steps = env.get_steps(group_name)
    # We send data to Unity : A string with the number of Agent at each
    string_log.send_string(
        f"Step {i} occurred with {len(decision_steps)} deciding agents and "
        f"{len(terminal_steps)} terminal agents")
    env.step()  # Move the simulation forward

env.close()
