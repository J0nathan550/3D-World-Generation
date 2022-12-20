public class Rnd
{
    int _rnd = 0;

    public Rnd(int seed)
    {
        _rnd = seed;
        for (int i = 0; i < _rnd; i++) NextRnd();
    }

    /// <summary>
    /// Generates a random noise number between 0 and 1
    /// </summary>
    /// <returns></returns>
    public float Get()
    {
        NextRnd();
        int v = _rnd;
        if (v < 0) v *= -1;
        return v / (float)int.MaxValue;
    }

    /// <summary>
    /// Generates a random noise number between min and max
    /// </summary>
    /// <returns></returns>
    public float Get(float min, float max)
    {
        NextRnd();
        int v = _rnd;
        if (v < 0) v *= -1;
        float res = v / (float)int.MaxValue;
        return res * (max - min) + min;
    }
    /// <summary>
    /// Generates a random integer noise number between min and max. Max is excluded.
    /// </summary>
    /// <returns></returns>
    public int Get(int min, int max)
    {
        NextRnd();
        int v = _rnd;
        if (v < 0) v *= -1;
        int res = (int)((v / (float)int.MaxValue) * (max - min) + min);
        if (res == max) res = min + (max - min) / 2;
        return res;
    }

    private void NextRnd()
    {
        byte a = (byte)((_rnd & 0xff000000) >> 24);
        _rnd *= 129;
        _rnd ^= 0b0101100110011010110011010101;
        _rnd *= 129;
        _rnd ^= a;
        _rnd++;
    }
}