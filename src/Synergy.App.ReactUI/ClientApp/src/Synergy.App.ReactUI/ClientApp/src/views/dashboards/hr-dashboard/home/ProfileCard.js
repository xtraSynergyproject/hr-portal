// ** React Imports
import { useEffect, useState } from 'react'

// ** MUI Imports
import Box from '@mui/material/Box'
import Grid from '@mui/material/Grid'
import Card from '@mui/material/Card'
import Link from '@mui/material/Link'
import Table from '@mui/material/Table'
import Button from '@mui/material/Button'
import Avatar from '@mui/material/Avatar'
import Dialog from '@mui/material/Dialog'
import Tooltip from '@mui/material/Tooltip'
import Checkbox from '@mui/material/Checkbox'
import TableRow from '@mui/material/TableRow'
import TableBody from '@mui/material/TableBody'
import TableCell from '@mui/material/TableCell'
import TableHead from '@mui/material/TableHead'
import TextField from '@mui/material/TextField'
import IconButton from '@mui/material/IconButton'
import Typography from '@mui/material/Typography'
import FormControl from '@mui/material/FormControl'
import DialogTitle from '@mui/material/DialogTitle'
import AvatarGroup from '@mui/material/AvatarGroup'
import CardContent from '@mui/material/CardContent'
import DialogActions from '@mui/material/DialogActions'
import DialogContent from '@mui/material/DialogContent'
import TableContainer from '@mui/material/TableContainer'
import FormControlLabel from '@mui/material/FormControlLabel'

// ** Icon Imports
import Icon from 'src/@core/components/icon'

const cardData = [
    { avatars: ['1.png', '2.png', '3.png', '4.png'], title: 'Pooja Limbola', avatars: ['1.png', '2.png', '3.png', '4.png', '1.png', '2.png', '3.png', '4.png'] },

]



const ProfileCard = () => {
    // ** States
    const [open, setOpen] = useState(false)
    const [dialogTitle, setDialogTitle] = useState('Add')
    const [selectedCheckbox, setSelectedCheckbox] = useState([])
    const [isIndeterminateCheckbox, setIsIndeterminateCheckbox] = useState(false)
    const handleClickOpen = () => setOpen(true)

    const handleClose = () => {
        setOpen(false)
        setSelectedCheckbox([])
        setIsIndeterminateCheckbox(false)
    }

    const togglePermission = id => {
        const arr = selectedCheckbox
        if (selectedCheckbox.includes(id)) {
            arr.splice(arr.indexOf(id), 1)
            setSelectedCheckbox([...arr])
        } else {
            arr.push(id)
            setSelectedCheckbox([...arr])
        }
    }

    const handleSelectAllCheckbox = () => {
        if (isIndeterminateCheckbox) {
            setSelectedCheckbox([])
        } else {
            rolesArr.forEach(row => {
                const id = row.toLowerCase().split(' ').join('-')
                togglePermission(`${id}-read`)
                togglePermission(`${id}-write`)
                togglePermission(`${id}-create`)
            })
        }
    }
    useEffect(() => {
        if (selectedCheckbox.length > 0 && selectedCheckbox.length < rolesArr.length * 3) {
            setIsIndeterminateCheckbox(true)
        } else {
            setIsIndeterminateCheckbox(false)
        }
    }, [selectedCheckbox])

    const renderCards = () =>
        cardData.map((item, index) => (
            <Grid item xs={12} sm={6} lg={9} key={index} sx={{ maxWidth: 900, flexGrow: 1, borderRadius: 5, margin: 'auto' }}>
                <Card 
                  sx={{
                   
                    boxShadow: theme => `${theme.shadows[0]} !important`,
                  
                }}>
                    <CardContent>
                        <Box sx={{ mb: 1, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>

                            <Grid item xs={3} sx={{mt:5}}>
                                <Box sx={{ height: '100%', display: 'flex', alignItems: 'flex-end', justifyContent: 'center' }}>

                                    <img style={{ borderRadius: "150px" }} width={200} height={200} alt='add-role' src='/images/avatars/1.png' />
                                </Box>
                            </Grid>

                            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-end' }}>
                                <Box sx={{ display: 'flex', flexDirection: 'column' }}>
                                    <Typography variant='h5' sx={{ paddingRight: 5 }}>{item.title}</Typography>
                                    <Typography
                                        href='/'
                                        variant='body2'
                                        component={Link}
                                        sx={{ color: 'primary.main' }}
                                        onClick={e => {
                                            e.preventDefault()
                                            handleClickOpen()
                                            setDialogTitle('Edit')
                                        }}
                                    >
                                        <h3>431 Requested</h3>
                                    </Typography>
                                    <AvatarGroup max={4} sx={{ '& .MuiAvatar-root': { width: 50, height: 50, fontSize: '0.875rem' } }}>
                                        {item.avatars.map((img, index) => (
                                            <Avatar key={index} alt={item.title} src={`/images/avatars/${img}`} />
                                        ))}
                                    </AvatarGroup>
                                </Box>
                                <IconButton sx={{ color: 'text.secondary', fontWeight: 'bold' }}>
                                    <Icon icon='mdi:content-copy' fontSize={20} />
                                </IconButton>
                            </Box >

                            <Box sx={{width: '50%'}}>
                            <Button
                                variant='contained'
                                sx={{ m: 1, whiteSpace: 'nowrap' }}
                                onClick={() => {
                                    handleClickOpen()
                                    setDialogTitle('Add')
                                }}
                            >
                                +Add To Story
                            </Button>

                            <Button
                                variant='contained'
                                sx={{ m:1, whiteSpace: 'nowrap' }}
                                onClick={() => {
                                    handleClickOpen()
                                    setDialogTitle('Add')
                                }}
                            >
                                Edit Profile
                            </Button>
                            </Box>
                        </Box>
                    </CardContent>

                </Card>
            </Grid>
        ))

    return (
        <Grid container spacing={6} className='match-height'>
            {renderCards()}

            <Dialog fullWidth maxWidth='md' scroll='body' onClose={handleClose} open={open}>
                <DialogTitle sx={{ textAlign: 'center' }}>
                    <Typography variant='h5' component='span'>
                        {`${dialogTitle} Role`}
                    </Typography>
                    <Typography variant='body2'>Set Role Permissions</Typography>
                </DialogTitle>
                <DialogContent sx={{ p: { xs: 6, sm: 12 } }}>
                    <Box sx={{ my: 4 }}>
                        <FormControl fullWidth>
                            <TextField label='Role Name' placeholder='Enter Role Name' />
                        </FormControl>
                    </Box>
                    <Typography variant='h6'>Role Permissions</Typography>
                    <TableContainer>
                        <Table size='small'>
                            <TableHead>
                                <TableRow>
                                    <TableCell sx={{ pl: '0 !important' }}>
                                        <Box
                                            sx={{
                                                display: 'flex',
                                                fontSize: '0.875rem',
                                                whiteSpace: 'nowrap',
                                                alignItems: 'center',
                                                textTransform: 'capitalize',
                                                '& svg': { ml: 1, cursor: 'pointer' }
                                            }}
                                        >
                                            <h5 >Pooja Limbola</h5>
                                            <Tooltip placement='top' title='Allows a full access to the system'>
                                                <Box sx={{ display: 'flex' }}>
                                                    <Icon icon='mdi:information-outline' fontSize='1rem' />
                                                </Box>
                                            </Tooltip>
                                        </Box>
                                    </TableCell>
                                    <TableCell colSpan={3}>
                                        <FormControlLabel
                                            label='Select All'
                                            sx={{ '& .MuiTypography-root': { textTransform: 'capitalize' } }}
                                            control={
                                                <Checkbox
                                                    size='small'
                                                    onChange={handleSelectAllCheckbox}
                                                    indeterminate={isIndeterminateCheckbox}

                                                />
                                            }
                                        />
                                    </TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>


                                return (
                                <TableRow sx={{ '& .MuiTableCell-root:first-of-type': { pl: '0 !important' } }}>
                                    <TableCell
                                        sx={{
                                            fontWeight: 600,
                                            whiteSpace: 'nowrap',
                                            color: theme => `${theme.palette.text.primary} !important`
                                        }}
                                    >

                                    </TableCell>


                                    <TableCell>
                                        <FormControlLabel
                                            label='Write'
                                            control={
                                                <Checkbox
                                                    size='small'


                                                />
                                            }
                                        />
                                    </TableCell>

                                </TableRow>
                                )

                            </TableBody>
                        </Table>
                    </TableContainer>
                </DialogContent>
                <DialogActions sx={{ pt: 0, display: 'flex', justifyContent: 'center' }}>
                    <Box className='demo-space-x'>
                        <Button size='large' type='submit' variant='contained' onClick={handleClose}>
                            Submit
                        </Button>
                        <Button size='large' color='secondary' variant='outlined' onClick={handleClose}>
                            Cancel
                        </Button>
                    </Box>
                </DialogActions>
            </Dialog>
        </Grid>
    )
}

export default ProfileCard
