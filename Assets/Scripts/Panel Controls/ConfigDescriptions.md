# Racing Game Configuration Variables

This document describes all configuration variables that control the vehicle physics, handling, and camera effects in the racing game.

## Physics & Movement

### Acceleration
- **Description**: Controls how quickly the car accelerates and reaches top speed.
- **Range**: 0-100
- **Default**: 17.0
- **Effect**: Higher values create more responsive acceleration and higher top speeds. Lower values make the car feel heavier and slower to respond.

### Angular Velocity
- **Description**: Controls how quickly the car can rotate/turn.
- **Range**: -5 to 5
- **Default**: -2.2
- **Effect**: Affects turning radius and responsiveness. Higher absolute values make sharper turns possible but may make the car harder to control.

### Forwards Friction
- **Description**: Resistance to movement in the forward direction.
- **Range**: 0-0.1
- **Default**: 0.04
- **Effect**: Higher values increase braking effect and make the car decelerate faster. Lower values give a "slippery" forward feeling.

### Sideways Friction
- **Description**: Resistance to lateral (side-to-side) movement.
- **Range**: 0-20
- **Default**: 10
- **Effect**: Higher values increase grip during turns and reduce sliding. Lower values make drifting easier but can make the car feel unstable.

### Air Resistance
- **Description**: How much the car slows down due to air resistance.
- **Range**: 0-0.05
- **Default**: 0.003
- **Effect**: Affects deceleration at high speeds. Higher values create a more pronounced slowing effect as speed increases.

### Gravity Value
- **Description**: The strength of gravity applied to the car.
- **Range**: 0-30
- **Default**: 15.685
- **Effect**: Affects how quickly the car falls and how it behaves on slopes. Higher values make the car feel heavier and more grounded.

### Suspension Distance
- **Description**: How far the wheels can extend from the car body.
- **Range**: 0-1
- **Default**: 0.5
- **Effect**: Affects how the car handles bumps and uneven terrain. Higher values allow better absorption of bumps but may make the car feel "bouncy".

## Steering Controls

### Steering A
- **Description**: Scaling factor in the steering formula.
- **Range**: -5 to 5
- **Default**: 1.5
- **Effect**: Affects overall steering sensitivity. Higher values make steering more responsive.

### Steering B
- **Description**: Denominator factor in the steering formula.
- **Range**: -0.1 to 0.1
- **Default**: 0.05
- **Effect**: Controls how quickly steering response diminishes at higher speeds. Higher values reduce steering effectiveness at high speeds.

### Steering C
- **Description**: Input scaling factor in the steering formula.
- **Range**: -0.5 to 0.5
- **Default**: 0.297
- **Effect**: Fine-tunes the steering response curve. Adjusts sensitivity throughout the steering range.

### Turn Bias Factor
- **Description**: Affects additional turning force during drifting.
- **Range**: 0-5
- **Default**: 0.0
- **Effect**: Higher values increase the bias in turning direction during drifts, making drifts more pronounced.

### Turn Bias Bias
- **Description**: Constant bias added to turning during drifts.
- **Range**: 0-5
- **Default**: 0.513
- **Effect**: Higher values make drifts more stable and predictable but may make them harder to initiate.

## Nitro System

### Nitro Factor
- **Description**: Acceleration multiplier when nitro is active.
- **Range**: 1-5
- **Default**: 2.0
- **Effect**: How much faster the car accelerates with nitro. Higher values give more dramatic speed boosts.

### Nitro Duration
- **Description**: How long (in seconds) nitro boost lasts.
- **Range**: 0-10
- **Default**: 2.0
- **Effect**: Longer durations allow extended boost periods but should be balanced with cooldown.

### Nitro Cool Down
- **Description**: Waiting time (in seconds) before nitro can be used again.
- **Range**: 0-10
- **Default**: 5.0
- **Effect**: Controls nitro usage frequency. Lower values allow more frequent use but may make the game less balanced.

## Camera Effects

### Tilt Input Multiplier
- **Description**: Multiplier for accelerometer input (mobile controls).
- **Range**: 0-5
- **Default**: 2.0
- **Effect**: Higher values increase steering sensitivity when using tilt controls.

### Tilt Amount
- **Description**: Maximum camera tilt angle during turns.
- **Range**: 0-180
- **Default**: 96.83
- **Effect**: Controls how much the camera rotates along the z-axis when turning. Higher values give a more dramatic visual effect.

### Shift Amount
- **Description**: How far the camera shifts position during turns.
- **Range**: 0-20
- **Default**: 3.372
- **Effect**: Controls lateral camera movement when turning. Higher values create a more pronounced sensation of movement.

### Shift Lerp Rate
- **Description**: How quickly the camera shift effect interpolates.
- **Range**: 0-20
- **Default**: 3.519
- **Effect**: Controls smoothness of camera position shifting. Higher values make camera movements more responsive but potentially jerky.

### Tilt Lerp Rate
- **Description**: How quickly the camera tilt effect interpolates.
- **Range**: 0-20
- **Default**: 3.519
- **Effect**: Controls smoothness of camera rotation. Higher values make tilting more responsive but potentially jerky.