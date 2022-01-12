import uuid

import numpy as np
from mlagents_envs.environment import UnityEnvironment
from mlagents_envs.side_channel.side_channel import (IncomingMessage,
                                                     OutgoingMessage,
                                                     SideChannel)


# Create the StringLogChannel class
class StringLogChannel(SideChannel):
    def __init__(self) -> None:
        super().__init__(uuid.UUID("e951342c-4f7e-11ea-b238-784f4387d1f7"))

    def on_message_received(self, msg: IncomingMessage) -> None:
        """
        Note: We must implement this method of the SideChannel interface to
        receive messages from Unity
        """
        # We simply read a string from the message and print it.
        print("on_message_received:", msg.read_string())

    def send_string(self, data: str) -> None:
        # Add the string to an OutgoingMessage
        msg = OutgoingMessage()
        msg.write_string(data)
        # We call this method to queue the data we want to send
        super().queue_message_to_send(msg)
