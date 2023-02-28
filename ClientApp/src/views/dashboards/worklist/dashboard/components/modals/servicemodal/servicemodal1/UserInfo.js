import React from 'react'

function UserInfo() {
  return (
    <div>
        <TextField
                      required
                      size='small'
                      sx={{ marginBottom: '20px' }}
                      id='date'
                      label='Leave Start Date*'
                      type='date'
                      defaultValue='YYYY-MM-DD'
                      InputLabelProps={{
                        shrink: true
                      }}
                    />

      
    </div>
  )
}

export default UserInfo
