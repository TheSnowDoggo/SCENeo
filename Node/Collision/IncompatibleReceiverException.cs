namespace SCENeo.Node.Collision;

internal class IncompatibleReceiverException(IReceive receiver)
    : Exception($"No collision implementation defined for receiver {receiver}.")
{ }