param([string]$Root = $PSScriptRoot)

Add-Type -AssemblyName System.Drawing

function Export-Png {
    param([string]$Source, [string]$Destination, [int]$Width, [int]$Height)
    $sourceImage = [System.Drawing.Image]::FromFile($Source)
    try {
        $targetRatio = $Width / $Height
        $cropWidth = [int][Math]::Min($sourceImage.Width, [Math]::Round($sourceImage.Height * $targetRatio))
        $cropHeight = [int][Math]::Min($sourceImage.Height, [Math]::Round($sourceImage.Width / $targetRatio))
        $crop = [System.Drawing.Rectangle]::new(
            [int](($sourceImage.Width - $cropWidth) / 2),
            [int](($sourceImage.Height - $cropHeight) / 2),
            $cropWidth,
            $cropHeight)
        $bitmap = [System.Drawing.Bitmap]::new($Width, $Height)
        try {
            $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
            try {
                $graphics.Clear([System.Drawing.Color]::Transparent)
                $graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
                $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::HighQuality
                $graphics.DrawImage($sourceImage, [System.Drawing.Rectangle]::new(0, 0, $Width, $Height), $crop, [System.Drawing.GraphicsUnit]::Pixel)
            } finally { $graphics.Dispose() }
            $bitmap.Save($Destination, [System.Drawing.Imaging.ImageFormat]::Png)
        } finally { $bitmap.Dispose() }
    } finally { $sourceImage.Dispose() }
}

$play = Join-Path $Root 'Play'
New-Item -ItemType Directory -Force -Path $play | Out-Null
Export-Png (Join-Path $Root 'Source/cloudora-icon-master.png') (Join-Path $play 'cloudora-icon-512.png') 512 512
Export-Png (Join-Path $Root 'Source/cloudora-feature-master.png') (Join-Path $play 'cloudora-feature-1024x500.png') 1024 500

$icon = [System.Drawing.Image]::FromFile((Join-Path $Root 'Source/cloudora-icon-master.png'))
try {
    $foreground = [System.Drawing.Bitmap]::new(512, 512)
    try {
        $graphics = [System.Drawing.Graphics]::FromImage($foreground)
        try {
            $graphics.Clear([System.Drawing.Color]::Transparent)
            $graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
            $graphics.DrawImage($icon, [System.Drawing.Rectangle]::new(85, 85, 342, 342))
        } finally { $graphics.Dispose() }
        $foreground.Save((Join-Path $play 'cloudora-adaptive-foreground-512.png'), [System.Drawing.Imaging.ImageFormat]::Png)
    } finally { $foreground.Dispose() }
} finally { $icon.Dispose() }

$background = [System.Drawing.Bitmap]::new(512, 512)
try {
    $graphics = [System.Drawing.Graphics]::FromImage($background)
    try {
        $graphics.Clear([System.Drawing.ColorTranslator]::FromHtml('#EAF6FF'))
    } finally { $graphics.Dispose() }
    $background.Save((Join-Path $play 'cloudora-adaptive-background-512.png'), [System.Drawing.Imaging.ImageFormat]::Png)
} finally { $background.Dispose() }
