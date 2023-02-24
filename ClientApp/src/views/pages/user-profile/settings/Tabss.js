// ** MUI Imports
import Box from '@mui/material/Box'
import Tab from '@mui/material/Tab'
import TabList from '@mui/lab/TabList'
import TabPanel from '@mui/lab/TabPanel'
import TabContext from '@mui/lab/TabContext'
import Typography from '@mui/material/Typography'

import Switch from '@mui/material/Switch'
import Button from '@mui/material/Button'
import Stack from '@mui/material/Stack'
import TextField from '@mui/material/TextField'
import Grid from '@mui/material/Grid'
import { CardContent, Divider } from '@mui/material'
import Card from '@mui/material/Card'
import { useState } from 'react'

//start change1
// ** React Imports
import Link from '@mui/material/Link'
import { styled } from '@mui/material/styles'
// ** Third Party Imports
import { useDropzone } from 'react-dropzone'
import Spacing from 'src/@core/theme/spacing'
//end change1

//start change2
// Styled component for the upload image inside the dropzone area
const Img = styled('img')(({ theme }) => ({
  [theme.breakpoints.up('md')]: {
    marginRight: theme.spacing(15.75)
  },
  [theme.breakpoints.down('md')]: {
    marginBottom: theme.spacing(4)
  },
  [theme.breakpoints.down('sm')]: {
    width: 160
  }
}))

// Styled component for the heading inside the dropzone area
const HeadingTypography = styled(Typography)(({ theme }) => ({
  marginBottom: theme.spacing(5),
  [theme.breakpoints.down('sm')]: {
    marginBottom: theme.spacing(4)
  }
}))

//end change2
const label = { inputProps: { 'aria-label': 'Switch demo' } }
const Tabs = () => {
  // ** State
  const [value, setValue] = useState('1')

  const handleChange = (event, newValue) => {
    setValue(newValue)
  }

  //start change3
  // ** State
  const [files, setFiles] = useState([])

  // ** Hook
  const { acceptedFiles, getRootProps, getInputProps } = useDropzone({
    multiple: false,
    accept: {
      'image/*': ['.png', '.jpg', '.jpeg', '.gif']
    },
    onDrop: acceptedFiles => {
      setFiles(acceptedFiles.map(file => Object.assign(file)))
    }
  })

  const handleLinkClick = event => {
    event.preventDefault()
  }

  const img = files.map(file => (
    <img key={file.name} alt={file.name} height='300px' width='300px' src={URL.createObjectURL(file)} />
  ))
  //end change3
  return (
    <Card>
      <CardContent>
        <Grid container spacing={12}>
          <Grid item xs={12}>
            <Grid className='UDDmaingridTabbs' item xs={12}>
              <TabContext value={value}>
                <Grid sx={{ height: 'auto', width: '300px' }}>
                  <TabList orientation='horizontal' onChange={handleChange} aria-label='vertical tabs example'>
                    <Tab value='1' label='profile' />
                    <Tab value='2' label='Preferences' />
                  </TabList>
                  <TabPanel value='1'>
                    <Grid>
                      <Typography>
                        <b>Administrator</b>
                      </Typography>
                      <br />
                      {/* start change4 */}
                      <Grid
                        {...getRootProps({ className: 'dropzone' })}
                        sx={acceptedFiles.length ? { height: 300 } : {}}
                      >
                        <input {...getInputProps()} />
                        <Box sx={{ display: 'flex', flexDirection: ['column'], alignItems: 'left' }}>
                          {files.length ? img : null}
                          <Box
                            sx={{
                              display: 'flex',
                              flexDirection: 'column'
                            }}
                          >
                            {/* <Img alt='Upload img' src='/images/misc/upload.png' />
                         <HeadingTypography variant='h5'>Drop files here or click to upload.</HeadingTypography> */}
                            <Typography color='textSecondary'>
                              Drop files here or click
                              <span> </span>
                              <Link href='/' onClick={handleLinkClick}>
                                Browse file
                              </Link>
                            </Typography>
                          </Box>
                        </Box>
                      </Grid>
                      {/* end change4 */}
                      <br />
                      <br />
                      <Box>
                        Name:
                        <TextField fullWidth label='Administrator' id='outlined-full-width' sx={{ mb: 4 }} />
                        Job Title:
                        <TextField fullWidth label='System Administrator' id='outlined-full-width' sx={{ mb: 2 }} />
                        <Divider />
                        <Stack
                          direction='row'
                          alignItems='center'
                          spacing={2}
                          sx={{ display: 'flex', justifyContent: 'right',marginTop:'13px' }}
                        >
                          <Button variant='contained' component='label'>
                            Upload Profile
                          </Button>
                        </Stack>
                      </Box>
                    </Grid>
                  </TabPanel>
                  <TabPanel value='2'>
                    <Typography>Notifications</Typography>
                    <br />
                    <Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
                      <Typography>
                        <b> Enable Regular Email</b>
                        <Switch {...label} defaultChecked />
                      </Typography>
                    </Box>

                    <Typography>
                      <b> Enable Summary Email</b>
                      <Switch {...label} defaultChecked />
                    </Typography>

                    <Divider width='280px' />
                    <Stack direction='row' justifyContent='right' marginTop='13px'>
                      <Button
                        sx={{ display: 'flex', fontSize: '12px', textAlign: 'center', height: '48px', width: '100px' }}
                        variant='contained'
                        component='label'
                      >
                        Upload Preferences
                      </Button>
                    </Stack>
                  </TabPanel>
                </Grid>
              </TabContext>
            </Grid>
          </Grid>
        </Grid>
      </CardContent>
    </Card>
  )
}

export default Tabs
