# Car Configuration Variable Descriptions

This document explains the configuration variables used to control the car's movement, physics, nitro, and camera behavior. These variables are typically adjusted via sliders managed by `VariableSliderManager.cs`.

## Core Physics & Movement

*   **`acceleration`**: (Float)
    *   Determines the base rate at which the car gains speed when accelerating forward. Higher values mean faster acceleration.
    *   Used in: `CarHybrid.cs`, `CarSwiftPhys.cs`, `Car.cs`, `CarAI.cs`

*   **`angularVelocity`**: (Float)
    *   Controls the base speed at which the car rotates (turns) when steering input is applied. Higher values result in faster turning. This is modulated by the `SteeringValue` function based on speed.
    *   Used in: `CarHybrid.cs`, `CarSwiftPhys.cs`, `Car.cs`

*   **`forwardsFriction`**: (Float)
    *   Represents the force opposing the car's movement in its forward/backward direction when on the ground. Higher values cause the car to slow down faster when not accelerating.
    *   Used in: `CarHybrid.cs`, `CarSwiftPhys.cs`, `Car.cs`

*   **`sidewaysFriction`**: (Float)
    *   Represents the force opposing the car's movement sideways (resisting sliding/drifting) when on the ground. Higher values provide more grip and make drifting harder. This value might be dynamically adjusted during drifting.
    *   Used in: `CarHybrid.cs`, `CarSwiftPhys.cs`, `Car.cs`

*   **`airResistance`**: (Float)
    *   A factor determining how much the car's speed is reduced due to air drag. It's typically proportional to the square of the car's velocity. Higher values mean more drag, slowing the car down more significantly at higher speeds.
    *   Used in: `CarHybrid.cs`, `CarSwiftPhys.cs`, `Car.cs`, `CarController.cs`

*   **`gravityValue`**: (Float)
    *   The magnitude of the downward force applied to the car to simulate gravity. Standard gravity is approximately 9.81, but this can be tuned for gameplay.
    *   Used in: `CarHybrid.cs`, `CarSwiftPhys.cs` (Note: `Car.cs` doesn't seem to apply gravity explicitly, relying on Rigidbody settings. `CarController.cs` uses `downForce` instead).

*   **`suspensionDistance`**: (Float)
    *   Controls the maximum travel distance of the `WheelCollider`'s suspension along its local Y-axis. Affects how the car body reacts to bumps and terrain changes.
    *   Used in: `VariableSliderManager.cs` (sets `WheelCollider.suspensionDistance`)

## Steering & Drifting

*   **`steeringA`, `steeringB`, `steeringC`**: (Floats)
    *   Parameters used in the `SteeringValue(speed)` function, which calculates the effective steering angle based on the car's current speed. This function likely models how steering becomes less sensitive at higher speeds.
        *   `steeringA`: Seems to be a primary multiplier for the steering curve.
        *   `steeringB`, `steeringC`: Shape the curve, likely affecting how quickly sensitivity drops off with speed.
    *   Used in: `CarHybrid.cs`, `CarSwiftPhys.cs`, `Car.cs`

*   **`turnBiasFactor`**: (Float)
    *   A multiplier applied to the horizontal input when initiating a drift (often combined with braking/backing). It exaggerates the turn angle to help induce a drift.
    *   Used in: `CarHybrid.cs`, `CarSwiftPhys.cs`, `Car.cs`

*   **`turnBiasBias`**: (Float)
    *   An additional fixed angle added (with the sign of the horizontal input) when initiating a drift. Works with `turnBiasFactor` to force a sharper turn for drifting.
    *   Used in: `CarHybrid.cs`, `CarSwiftPhys.cs`, `Car.cs`

## Nitro Boost

*   **`nitroFactor`**: (Float)
    *   A multiplier applied to the car's acceleration (or added to vertical input in `CarHybrid`) when the nitro boost is active. A value of 2 means double acceleration during boost.
    *   Used in: `CarHybrid.cs`, `CarSwiftPhys.cs`, `Car.cs`, `CarController.cs`

*   **`nitroDuration`**: (Float)
    *   The length of time (in seconds) a single nitro boost lasts.
    *   Used in: `CarHybrid.cs`, `CarSwiftPhys.cs`, `Car.cs`

*   **`nitroCoolDown`**: (Float)
    *   The time (in seconds) the player must wait after a nitro boost ends before they can use it again (or before the charge starts regenerating, depending on implementation).
    *   Used in: `CarHybrid.cs`, `CarSwiftPhys.cs`, `Car.cs`, `CarController.cs` (as `nitroCooldown`)

## Camera & Input (Mobile Tilt)

*   **`tiltInputMultiplier`**: (Float)
    *   A multiplier applied to the raw accelerometer input (X-axis) to determine the horizontal steering input on mobile devices. Higher values make tilt steering more sensitive.
    *   Used in: `CarHybrid.cs`

*   **`tiltAmount`**: (Float)
    *   The maximum angle (in degrees) the camera will tilt (roll) left or right based on the phone's accelerometer input (X-axis).
    *   Used in: `CameraTiltAndShift.cs`

*   **`shiftAmount`**: (Float)
    *   The maximum distance the camera will shift horizontally (left/right) based on the phone's accelerometer input (X-axis).
    *   Used in: `CameraTiltAndShift.cs`

*   **`tiltLerpRate`**: (Float)
    *   Controls the speed at which the camera smoothly interpolates (lerps) towards the target tilt angle calculated from accelerometer input. Higher values mean faster, potentially snappier, tilting.
    *   Used in: `CameraTiltAndShift.cs`

*   **`shiftLerpRate`**: (Float)
    *   Controls the speed at which the camera smoothly interpolates (lerps) towards the target horizontal shift position calculated from accelerometer input. Higher values mean faster, potentially snappier, shifting.
    *   Used in: `CameraTiltAndShift.cs`
