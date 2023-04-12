namespace HoangCuongSneaker.Core.Enum
{
    /// <summary>
    /// các option lọc giá sản phẩm (class Product)
    /// </summary>
    public enum PriceRangeFilterEnum
    {
        LessThanOneMillion = 1, // dưới 1 triệu
        OneMillionToTwoMillion = 2, // từ 1 => 2 tr
        TwoMillionToThreeMillion = 3, // từ 2 => 3 tr
        ThreeMillionToFiveMillion = 4, // từ 3 => 5 tr
        GreaterThanFiveMillion = 5, // trên 5tr
    }
}