USE IT_ServiceCenter_Practice;
GO

-- Запрос 1. Полный список заявок
-- Номер заявки, описание, дата создания и стоимость, отсортированные от новых к старым.
SELECT 
    RequestId, 
    Description, 
    CreatedAt, 
    Cost
FROM dbo.ServiceRequests
ORDER BY CreatedAt DESC;

-- Запрос 2. Новые заявки
-- Номер заявки, описание и дата создания для заявок со статусом «Новая».
SELECT 
    sr.RequestId, 
    sr.Description, 
    sr.CreatedAt
FROM dbo.ServiceRequests AS sr
INNER JOIN dbo.RequestStatuses AS rs ON sr.StatusId = rs.StatusId
WHERE rs.StatusName = N'Новая';

-- Запрос 3. Дорогие сетевые обращения
-- Номер, описание, стоимость и категория «Сеть», где стоимость > 1000 рублей.
SELECT 
    sr.RequestId, 
    sr.Description, 
    sr.Cost, 
    c.CategoryName
FROM dbo.ServiceRequests AS sr
INNER JOIN dbo.Categories AS c ON sr.CategoryId = c.CategoryId
WHERE c.CategoryName = N'Сеть' AND sr.Cost > 1000.00;

-- Запрос 4. Поиск по фрагменту текста
-- Заявки, в описании которых встречается слово «принтер» в любой части.
SELECT 
    RequestId, 
    Description, 
    Cost
FROM dbo.ServiceRequests
WHERE Description LIKE N'%принтер%';

-- Запрос 5. Подробный отчёт по заявкам (Скриншот №1: JOIN)
-- Полный список полей. LEFT JOIN гарантирует, что заявка выведется, даже если сотрудник не назначен.
SELECT 
    sr.RequestId, 
    cl.FullName AS ClientName, 
    cat.CategoryName, 
    sr.Description, 
    st.StatusName, 
    emp.FullName AS EmployeeName, 
    sr.Cost
FROM dbo.ServiceRequests AS sr
INNER JOIN dbo.Clients AS cl ON sr.ClientId = cl.ClientId
INNER JOIN dbo.Categories AS cat ON sr.CategoryId = cat.CategoryId
INNER JOIN dbo.RequestStatuses AS st ON sr.StatusId = st.StatusId
LEFT JOIN dbo.Employees AS emp ON sr.EmployeeId = emp.EmployeeId;

-- Запрос 6. Заявки без назначенного сотрудника
-- Номер, описание, дата создания и ФИО клиента, отсортированные от старых к новым.
SELECT 
    sr.RequestId, 
    sr.Description, 
    sr.CreatedAt, 
    cl.FullName AS ClientName
FROM dbo.ServiceRequests AS sr
INNER JOIN dbo.Clients AS cl ON sr.ClientId = cl.ClientId
WHERE sr.EmployeeId IS NULL
ORDER BY sr.CreatedAt ASC;

-- Запрос 7. Количество заявок по статусам
-- Название статуса и количество заявок, отсортированное от большего к меньшему.
SELECT 
    rs.StatusName, 
    COUNT(sr.RequestId) AS RequestCount
FROM dbo.RequestStatuses AS rs
LEFT JOIN dbo.ServiceRequests AS sr ON rs.StatusId = sr.StatusId
GROUP BY rs.StatusName
ORDER BY RequestCount DESC;

-- Запрос 8. Анализ стоимости по категориям (Скриншот №2: GROUP BY/HAVING)
-- Средняя и максимальная стоимость заявок по категориям, где средняя стоимость > 1000 рублей.
SELECT 
    c.CategoryName, 
    AVG(sr.Cost) AS AverageCost, 
    MAX(sr.Cost) AS MaximumCost
FROM dbo.Categories AS c
INNER JOIN dbo.ServiceRequests AS sr ON c.CategoryId = sr.CategoryId
GROUP BY c.CategoryName
HAVING AVG(sr.Cost) > 1000.00;

-- Запрос 9. Клиенты с несколькими обращениями
-- ФИО клиента и количество заявок для тех, у кого зарегистрировано не менее двух обращений.
SELECT 
    c.FullName, 
    COUNT(sr.RequestId) AS RequestCount
FROM dbo.Clients AS c
INNER JOIN dbo.ServiceRequests AS sr ON c.ClientId = sr.ClientId
GROUP BY c.FullName
HAVING COUNT(sr.RequestId) >= 2
ORDER BY RequestCount DESC;

-- Запрос 10. Клиенты без заявок (Скриншот №3: LEFT JOIN + IS NULL)
-- Клиенты, которые ещё ни разу не создавали обращения.
SELECT 
    c.FullName, 
    c.City
FROM dbo.Clients AS c
LEFT JOIN dbo.ServiceRequests AS sr ON c.ClientId = sr.ClientId
WHERE sr.RequestId IS NULL;
