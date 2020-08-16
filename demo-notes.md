# Let's take a look at it in action
* Here's a basic Blazor hello world component
* Blazor is a component-based framework - so you build up a set of components, eg buttons, form inputs, together into your UI
* Components are written in `.razor` files
* Uses the Razor templating language - same as in server side MVC, if you've seen that before
* Write HTML, but can inject in C# using the `@` symbol
* Blazor compnonents can have the `@code` block - this is where you define actual logic - state, event handlers, lifecycle methods, business logic, etc
* This is all compiled down to a C# class

* *Demo running the site - show the demo loading*

* Can compose components. E.g. let's render my Counter component
* *Add `<Counter />` to index*
* My counter component maintains some internal state. See how we can bind a method to a DOM event, in this case the onClick event
* This updates the state, and causes the component to be re-rendered.
* *Demo Counter*

* Components can have input parameters set - e.g. we could allow customising the Increment amount:
`<Counter Increment="10" />`


# BlazorBites

Let's build a takeaway ordering site

We're going to get a list of dish options from an API server, let the user choose some dishes, then enter their details, then place the order.
This example is a little different - I'm hosting blazor from within a server. I haven't had to change anything on the Blazor side for this to work.

## Fetch dishes
In `OrderFlow.razor`:

Inject HttpClient:
```csharp
@inject HttpClient HttpClient
```

Fetch data when the component first renders:
```csharp
private IEnumerable<Dish> availableDishes;

 protected override async Task OnInitializedAsync()
  {
    availableDishes = await HttpClient.GetFromJsonAsync<IEnumerable<Dish>>("api/menu");
    status = OrderStatus.ChooseDishes;
  }
```

Display data once we've finished loading:
```csharp
else if (status == OrderStatus.ChooseDishes)
{
  <div>Got @availableDishes.Count() dishes</div>
}
```

## Display dish selection
Add `Menu.razor` to contain the selection logic

Add a parameter to accept the list of dishes:
```csharp
[Parameter]
public IEnumerable<Dish> Dishes { get; set; }
```

Render the existing `DishSelector` component for each dish:
```csharp
<div class="dish-selection">
  @foreach (var dish in Dishes)
  {
    <DishSelector Dish="@dish" />
  }
</div>
```

Render the `Menu` in `index.razor`
```csharp
  <Menu Dishes="@availableDishes" />
```

## Allow selecting dishes

Store selected dishes in the `Menu`'s internal state
```csharp
  private readonly ICollection<Dish> selectedDishes = new List<Dish>();

  private void OnToggleDishSelected(Dish dish)
  {
    if (selectedDishes.Contains(dish))
    {
      selectedDishes.Remove(dish);
    }
    else
    {
      selectedDishes.Add(dish);
    }
  }
```

Wire up the `DishSelector` to update our state:
```csharp
<DishSelector Dish="@dish" IsSelected="@selectedDishes.Contains(dish)" OnToggleSelected="@OnToggleDishSelected" />
```

## Confirm dishes
Add an `EventCallback` so that the `Menu` can communicate back to it's parent when the dishes have been selected:
```csharp
  [Parameter]
  public EventCallback<ICollection<Dish>> OnDishesSelected { get; set; }
  ```

Add a button to invoke this callback:
```csharp
<button
   class="place-order"
   disabled="@(selectedDishes.Count == 0)" 
   @onclick="@(() => OnDishesSelected.InvokeAsync(selectedDishes))">
    Place order
</button>
```

Make the `OrderFlow.razor` receive this callback and store the selected dishes
```csharp
  private readonly ICollection<Dish> selectedDishes = new List<Dish>();

  private void OnDishesSelected(IEnumerable<Dish> dishes)
  {
    order.Dishes = dishes;
    status = OrderStatus.OrderDetails;
  }
```

```csharp
  <Menu Dishes="@availableDishes" OnDishesSelected="@OnDishesSelected" />
```


## Order details form
Create an `OrderDetails.razor` file
```csharp
<EditForm Model="Order">
  <label>Your name:
    <InputText @bind-Value="@Order.CustomerName" />
  </label>
  <label>
    Your address:
    <InputText @bind-Value="@Order.CustomerAddress" />
  </label>
  <button type="submit">Confirm order</button>
</EditForm>

@code {
  [Parameter]
  public Order Order { get; set; }
}
```

Make `OrderFlow.razor` display this:
```csharp
else if (status == OrderStatus.OrderDetails)
{
  <OrderDetails Order="@order" />
}
```

Allow submitting the order:
```csharp
@inject HttpClient HttpClient

...

<EditForm Model="@Order" OnValidSubmit="@OnValidSubmit">

...

<button type="submit">Confirm order</button>

```

```csharp

  private async Task OnValidSubmit()
  {
    var response = await HttpClient.PostAsJsonAsync("api/order", Order);
  }
  ```

## Add validation
Show off annotations on `Order.cs`

Add validation components:
```csharp
  <DataAnnotationsValidator />

...

  <ValidationMessage For="@(() => Order.CustomerName)" />

...

  <ValidationMessage For="@(() => Order.CustomerAddress)" />

```

Demo not being able to submit invalid data

## Handle succesful order
```csharp
  [Parameter]
  public EventCallback OnOrderConfirmed { get; set; }


  private async Task OnValidSubmit()
  {
    var response = await HttpClient.PostAsJsonAsync("api/order", Order);
    await OnOrderConfirmed.InvokeAsync(null);
  }
```

Make `OrderFlow.razor` display the completed order:
```csharp
  private void OnOrderConfirmed()
  {
    status = OrderStatus.OrderConfirmed;
  }
```

```csharp
else if (status == OrderStatus.OrderDetails)
{
  <OrderDetails Order="@order" OnOrderConfirmed="@OnOrderConfirmed" />
}
else if (status == OrderStatus.OrderConfirmed)
{
  <div class="order-confirmed">Your order is on its way!</div>
}
```