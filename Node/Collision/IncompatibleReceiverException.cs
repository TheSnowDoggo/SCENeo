namespace SCENeo.Node.Collision;

internal class IncompatibleReceiverException(IReceiver receiver)
    : Exception($"No collision implementation defined for receiver {receiver}.")
{
}