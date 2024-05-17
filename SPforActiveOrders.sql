CREATE PROCEDURE GetActiveOrders_By_Customer
    @CustomerId UNIQUEIDENTIFIER
AS
BEGIN
    SELECT
        o.OrderId,
        o.OrderStatus,
        o.OrderType,
        o.OrderedOn,
		O.OrderBy,
        o.ShippedOn,
        p.ProductName,
        p.UnitPrice,
		p.ProductId,
		s.SupplierId,
        s.SupplierName
    FROM
        [Order] o
    INNER JOIN
        Product p ON o.ProductId = p.ProductId
    INNER JOIN
        Supplier s ON p.SupplierId = s.SupplierId
    WHERE
        o.IsActive = 1
    AND
        o.OrderBy = @CustomerId;
END;