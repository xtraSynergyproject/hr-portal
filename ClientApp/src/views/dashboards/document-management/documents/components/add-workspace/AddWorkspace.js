import { Close, TabOutlined } from '@mui/icons-material'
import { Box, Button, MenuItem, TextField, Typography } from '@mui/material'
import axios from 'axios'
import { useEffect, useState } from 'react'

const AddWorkspace = ({ toggleDrawer }) => {
  let [documentTemplate, setDocumentTemplate] = useState([])
  let [dataGrid, setDataGrid] = useState([])
  let [fullWidth, setFullWidth] = useState(false)
  let [legalEntity, setLegalEntity] = useState('')
  let [parentWorkspace, setParentWorkspace] = useState('')

  const fetchDocumentTemplate = async () => {
    try {
      const response = await axios.get(
        'https://webapidev.aitalkx.com/dms/workspace/GetDocumentTemplate?userId=45bba746-3309-49b7-9c03-b5793369d73c&portalName=HR'
      )
      console.log(response.data)
      setDocumentTemplate(response.data)
    } catch (error) {
      console.log(error)
    }
  }

  const fetchDataGrid = async () => {
    try {
      const response = await axios.get(
        'https://webapidev.aitalkx.com/dms/workspace/ReadDataGrid?userId=45bba746-3309-49b7-9c03-b5793369d73c&portalName=HR'
      )
      console.log(response.data)
      setDataGrid(response.data)
    } catch (error) {
      console.log(error)
    }
  }
  //   useEffect(() => {
  //     fetchDocumentTemplate();
  //     fetchDataGrid();
  //   }, []);

  const handleSubmit = e => {
    e.preventDefault()
    console.log('information submitted')
  }

  const handleTemplateChange = e => {
    console.log(e.target.value)
    setDocumentTemplate(e.target.value)
  }
  const handleLegalEntity = e => {
    setLegalEntity(e.target.value)
  }

  const handleParentWorkspace = e => {
    setParentWorkspace(e.target.value)
  }

  return (
    <Box sx={{ width: fullWidth ? '100vw' : '350px' }}>
      <Box
        sx={{
          padding: '10px',
          display: 'flex',
          justifyContent: 'space-between',
          borderBottom: '1px solid black'
        }}
      >
        <Typography variant='h6'>Manage Workspace</Typography>
        <Box>
          <TabOutlined sx={{ cursor: 'pointer' }} onClick={() => setFullWidth(!fullWidth)} />
          <Close sx={{ marginLeft: '10px', cursor: 'pointer' }} onClick={() => toggleDrawer(false)} />
        </Box>
      </Box>
      <form onSubmit={handleSubmit}>
        <Box
          sx={{
            display: 'flex',
            flexDirection: 'column',
            gap: '20px',
            padding: '15px'
          }}
        >
          <TextField label='Legal Entity' required select value={legalEntity} onChange={handleLegalEntity}>
            <MenuItem value='A'>A</MenuItem>
            <MenuItem value='B'>B</MenuItem>
            <MenuItem value='C'>C</MenuItem>
            <MenuItem value='D'>D</MenuItem>
            <MenuItem value='E'>E</MenuItem>
          </TextField>
          <TextField label='Parent Workspace' required select value={parentWorkspace} onChange={handleParentWorkspace}>
            <MenuItem value='A'>A</MenuItem>
            <MenuItem value='B'>B</MenuItem>
            <MenuItem value='C'>C</MenuItem>
            <MenuItem value='D'>D</MenuItem>
            <MenuItem value='E'>E</MenuItem>
          </TextField>
          <TextField label='Workspace Name' />
          <TextField label='Code' />
          <TextField
            label='Document Type'
            value={documentTemplate}
            onChange={handleTemplateChange}
            fullWidth
            select
            SelectProps={{
              multiple: true
            }}
          >
            <MenuItem value='A'>A</MenuItem>
            <MenuItem value='B'>B</MenuItem>
            <MenuItem value='C'>C</MenuItem>
          </TextField>
          <TextField label='Sequence Order' />
          <Box sx={{ display: 'flex', justifyContent: 'flex-end', gap: '10px' }}>
            <Button sx={{ cursor: 'pointer' }} variant='outlined' onClick={() => toggleDrawer(false)}>
              Close
            </Button>
            <Button sx={{ cursor: 'pointer' }} type='submit' variant='contained'>
              Save
            </Button>
          </Box>
        </Box>
      </form>
    </Box>
  )
}

export default AddWorkspace
