# Generated by the protocol buffer compiler.  DO NOT EDIT!
# source: face.proto

import sys
_b=sys.version_info[0]<3 and (lambda x:x) or (lambda x:x.encode('latin1'))
from google.protobuf import descriptor as _descriptor
from google.protobuf import message as _message
from google.protobuf import reflection as _reflection
from google.protobuf import symbol_database as _symbol_database
# @@protoc_insertion_point(imports)

_sym_db = _symbol_database.Default()




DESCRIPTOR = _descriptor.FileDescriptor(
  name='face.proto',
  package='face',
  syntax='proto2',
  serialized_options=None,
  serialized_pb=_b('\n\nface.proto\x12\x04\x66\x61\x63\x65\"\x16\n\x04\x46\x61\x63\x65\x12\x0e\n\x06isFace\x18\x01 \x02(\x05')
)




_FACE = _descriptor.Descriptor(
  name='Face',
  full_name='face.Face',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
    _descriptor.FieldDescriptor(
      name='isFace', full_name='face.Face.isFace', index=0,
      number=1, type=5, cpp_type=1, label=2,
      has_default_value=False, default_value=0,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  serialized_options=None,
  is_extendable=False,
  syntax='proto2',
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=20,
  serialized_end=42,
)

DESCRIPTOR.message_types_by_name['Face'] = _FACE
_sym_db.RegisterFileDescriptor(DESCRIPTOR)

Face = _reflection.GeneratedProtocolMessageType('Face', (_message.Message,), dict(
  DESCRIPTOR = _FACE,
  __module__ = 'face_pb2'
  # @@protoc_insertion_point(class_scope:face.Face)
  ))
_sym_db.RegisterMessage(Face)


# @@protoc_insertion_point(module_scope)
