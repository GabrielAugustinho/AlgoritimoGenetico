namespace AlgoritimoGenetico.Extensions
{
    public class ArrayComparer : IEqualityComparer<int[]>
    {
        public bool Equals(int[]? arr1, int[]? arr2)
        {
            if (arr1 == null || arr2 == null)
                return false;

            if (arr1.Length != arr2.Length)
                return false;

            for (int i = 0; i < arr1.Length; i++)
            {
                if (arr1[i] != arr2[i])
                    return false;
            }

            return true;
        }

        public int GetHashCode(int[] arr)
        {
            unchecked
            {
                int hash = 17;
                foreach (int num in arr)
                {
                    hash = hash * 23 + num.GetHashCode();
                }
                return hash;
            }
        }
    }
}
