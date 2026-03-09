extends SubViewport

@export var point_light : PointLight2D = null

func _ready() -> void:
	await get_tree().process_frame
	await get_tree().process_frame
	
	var tex = get_texture()
	if tex == null:
		return
	(point_light.texture as ImageTexture).set_image(tex.get_image())
	
