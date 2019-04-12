## Customer:
کلیات کلاینت مشتری هست

| Method | Description |
| ------ | ------ |
| CustomerController.GetProfileInformation() | دریافت نام، نام خانوادگی، جنسیت، تاریخ تولد، ایمیل، شماره موبایل، عکس پروفایل مشتری |
| CustomerController.GetCustomerShoppingCarts() | دریافت سبد های خرید مشتری با جزییات داخلی آن ها |
| CustomerController.GetCustomerOrders() | دریافت سفارشات مشتری با جزییات داخلی آن ها |
| CustomerController.GetAllCosmetics() | دریافت تمامی آرایشی و بهداشتی های فروشگاه |
| CustomerController.GetAllMedicines() | دریافت تمامی داروهای های فروشگاه |
| CustomerController.LoginWithEmailAndPass() | ورود توسط ایمیل و رمز حساب و دریافت توکن |
| CustomerController.AddNewCustomer() | ساخت حساب جدید و دریافت توکن و افزوده شدن به هاب و ارسال اس ام اس کد فعال سازی شماره موبایل |
| CustomerController.EditExistingCustomerProfile() | ویرایش اطلاعات حساب - شماره موبایل و ایمیل تغییر کند، بایستی تایید شوند. |
| CustomerController.AddNewShoppingCart() | ساخت سبد جدید و ارائه آیدی |
| CustomerController.AddCreditToWallet() | شارژ اعتبار حساب با زرین پال |
| CustomerController.WalletInquiry() | نمایش اعتبار حساب |
| CustomerController.RequestService() | درخواست از داروخانه ها برای انجام سفارش مشتری |
| CustomerController.RequestPhoneLogin() | درخواست اس ام اس کد ورود به مشتری |
| CustomerController.VerifyPhoneLogin() | تایید کد ورود برای لاگین |
| CustomerController.RequestResetPassword() | درخواست تغییر رمز ورود توسط اس ام اس |
| CustomerController.VerifyResetPassword() | تایید کد تغییر رمز برای دریافت توکن موقت |
| CustomerController.ResetPassword() | تغییر رمز ورود |
| CustomerController.VerifyPhoneNumber() | تایید شماره موبایل (که بعد از ثبت نام رخ داده) |
| CustomerController.LatestNotifications() | دریافت آخرین نوتیفیکشن های دریافت نشده/ساخته شده |
| CustomerController.NotificationResponse() | تایید دریافت نوتیفیکشن |
| CustomerController.GrabTokenFromHeader() | 
### Customer service (extras here):
جزییات کلاینت مشتری هست

| Method | Description |
| ------ | ------ |
| CustomerService.LoginHandler() | تولید توکن جدید ورود/ارائه توکن فعلی درصورت احراز هویت موفق |
| CustomerService.TokenValidationHandler() | بررسی اعتبار توکن قبل از اجرای عملیات هر فانکشن |
| CustomerService.ResetPasswordHandler() | تولید توکن موقت تغییر رمز عبور |
| CustomerService.ResetPasswordTokenValidationHandler() | بررسی توکن موقت تغییر رمز عبور برای احراز هویت |
| CustomerService.GetCustomerId() | دریافت آیدی مشتری - برای بررسی و محدود کردن عملیات نسبت به شخصی که توکن معتبر ارائه کرده است. |
| CustomerService.GetCustomerResetPasswordId() | دریافت آیدی مشتری - برای بررسی و محدود کردن عملیات تغییر رمز ورود نسبت به شخصی که توکن معتبر ارائه کرده است. |
| CustomerService.IsValidEmail() | بررسی صحت فرمت ایمیل |

## Management:
کلیات کلاینت مدیریت هست

| Method | Description |
| ------ | ------ |
| ManagementController.AddNewMedicine() | افزودن داروی جدید به فروشگاه |
| ManagementController.EditMedicine() | ویرایش داروی فعلی فروشگاه |
| ManagementController.ShowAllMedicines() | نمایش تمامی دارو های موجود فروشگاه |

### Management service (extras here):
جزییات کلاینت مدیریت هست

## Pharmacy:
کلیات کلاینت داروخانه هست

| Method | Description |
| ------ | ------ |
| PharmacyController.GetDbStatus() | دریافت وضعیت دیتابیس |
| PharmacyController.GetDateTime() | دریافت تاریخ سرور |
| PharmacyController.GetServerState() | دریافت وضعیت سرور |
| PharmacyController.GetProfile() | دریافت نام، کد، ایمیل، شماره تلفن، عکس، آدرس، وضعیت ارائه خدمات، اعتبار، نام مدیر داروخانه |
| PharmacyController.GetOrders() | سفارشات انجام شده داروخانه |
| PharmacyController.GetScore() | امتیاز داروخانه |
| PharmacyController.GetSettles() | دریافت تسویه حساب های داروخانه |
| PharmacyController.SetANewSettle() | افزودن تسویه حساب جدید |
| PharmacyController.GetTopFivePharmacies() | دریافت ۵ داروخانه برتر |
| PharmacyController.LoginWithEmailAndPass() | لاگین توسط ایمیل داروخانه و رمز عبور |
| PharmacyController.ToggleStateOfPharmacy() | تغییر وضعیت ارائه خدمات و نمایش وضعیت نهایی |
| PharmacyController.NumberOfOrdersInThisWeek() | تعداد سفارشات انجام شده در هفته |
| PharmacyController.AverageTimeOfPackingInThisWeek() | متوسط زمانی اتمام سفارشات انجام شده در هفته |
| PharmacyController.ServiceResponse() | پاسخ به پیشنهاد انجام سفارش مشتری |
| PharmacyController.InvoiceDetails() | ارسال فاکتور سفارش و اس ام اس به مشتری |
| PharmacyController.Logout() | خروج از هاب و غیر فعال شدن |
| PharmacyController.LatestNotifications() | دریافت آخرین نوتیفیکشن های دریافت نشده/ساخته شده |
| PharmacyController.NotificationResponse() | تایید دریافت نوتیفیکشن |
### Pharmacy service (extras here):
جزییات کلاینت داروخانه هست

| Method | Description |
| ------ | ------ |
| PharmacyService.TokenValidationHandler() | بررسی اعتبار توکن قبل از اجرای عملیات هر فانکشن |

## Db:
مقدار دهی اولیه تیبل ها هست

| Method | Description |
| ------ | ------ |
| DbController.init() | پر کردن تیبل ها و نمایش وضعیت پر کردن هر تیبل |
| DbController.CustomerInit() | مقداردهی ماکِ تیبل مشتری ها |
| DbController.ManagerInit() | مقداردهی ماکِ تیبل مدیران داروخانه ها |
| DbController.PharmacyInit() | مقداردهی ماکِ تیبل داروخانه ها |
| DbController.CourierInit() | مقداردهی ماکِ تیبل پیک موتوری ها |
| DbController.MedicineInit() | مقداردهی ماکِ تیبل دارو های فروشگاه |
| DbController.CosmeticInit() | مقداردهی ماکِ تیبل آرایشی و بهداشتی های فروشگاه |
| DbController.NotationInit() | مقداردهی ماکِ تیبل توضیحاتِ سفارش (یا سبد خرید) |
| DbController.ShoppingCartInit() | مقداردهی ماکِ تیبل سبد های خرید |
| DbController.PrescriptionInit() | مقداردهی ماکِ تیبل نسخه ها |
| DbController.PrescriptionItemInit() | مقداردهی ماکِ تیبل آیتم های نسخه |
| DbController.OrderInit() | مقداردهی ماکِ تیبل سفارشات مشتری |
| DbController.MedicineShoppingCartInit() | مقداردهی ماکِ تیبل مپینگ های داروی فروشگاهی به سبد خرید || DbController.CosmeticShoppingCartInit() | مقداردهی ماکِ تیبل مپینگ های آرایشی و بهداشتیِ فروشگاهی به سبد خرید |
| DbController.ScoreInit() | مقداردهی ماکِ تیبل امتیازات داروخانه ها |
| DbController.OccurrenceInit() | مقداردهی ماکِ تیبل رویداد های مهم |
| DbController.AccountInit() | مقداردهی ماکِ تیبل حساب بانکی داروخانه ها |
| DbController.SettleInit() | مقداردهی ماکِ تیبل تسویه حساب ها |
| DbController.TextMessageInit() | مقداردهی ماکِ تیبل اس ام اس ها |

## Transaction:
برای تایید پرداخت و اجرای عملیات پس از پرداخت و کیف پول هست

| Method | Description |
| ------ | ------ |
| TransactionController.Order() | کم کردن پول از اعتبار حساب مشتری در صورت صحت و نمایش آن |
| TransactionController.Wallet() | افزودن پول به اعتبار حساب مشتری در صورت صحت و نمایش آن |
